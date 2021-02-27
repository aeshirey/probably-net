use libc::c_char;
use probably::frequency::HyperLogLog;

#[no_mangle]
pub extern "C" fn hll_new(error_rate: f64) -> *mut HyperLogLog {
    let hll = HyperLogLog::new(error_rate);
    let hll = Box::new(hll);
    Box::into_raw(hll)
}

#[no_mangle]
pub extern "C" fn hll_new_from_keys(error_rate: f64, key0: u64, key1: u64) -> *mut HyperLogLog {
    let hll = HyperLogLog::new_from_keys(error_rate, key0, key1);
    let hll = Box::new(hll);
    Box::into_raw(hll)
}

macro_rules! hll_insert {
    ($func: ident, $ty: ty) => {
        #[no_mangle]
        pub unsafe extern "C" fn $func(hll: *mut HyperLogLog, value: $ty) {
            assert!(!hll.is_null());
            let mut hll: Box<HyperLogLog> = Box::from_raw(hll);
            hll.insert(&value);
            Box::leak(hll);
        }
    };
}

hll_insert!(hll_insert_i8, i8);
hll_insert!(hll_insert_u8, u8);
hll_insert!(hll_insert_i16, i16);
hll_insert!(hll_insert_u16, u16);
hll_insert!(hll_insert_i32, i32);
hll_insert!(hll_insert_u32, u16);
hll_insert!(hll_insert_i64, i64);
hll_insert!(hll_insert_u64, u64);
hll_insert!(hll_insert_bool, bool);

#[no_mangle]
pub unsafe extern "C" fn hll_insert_str(hll: *mut HyperLogLog, value: *const c_char) {
    assert!(!hll.is_null());
    assert!(!value.is_null());
    let mut hll: Box<HyperLogLog> = Box::from_raw(hll);
    let value = std::ffi::CStr::from_ptr(value);
    hll.insert(&value);
    Box::leak(hll);
}

#[no_mangle]
pub unsafe extern "C" fn hll_len(hll: *mut HyperLogLog) -> f64 {
    assert!(!hll.is_null());
    let hll: Box<HyperLogLog> = Box::from_raw(hll);
    let len = Box::leak(hll);
    len.len()
}

#[no_mangle]
pub unsafe extern "C" fn hll_drop(hll: *mut HyperLogLog) {
    if !hll.is_null() {
        let hll: Box<HyperLogLog> = Box::from_raw(hll);
        std::mem::drop(hll);
    }
}
