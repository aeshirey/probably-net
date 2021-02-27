use libc::c_char;
use probably::frequency::HyperLogLog;
use std::mem::transmute;

#[no_mangle]
pub extern "C" fn hll_new(error_rate: f64) -> *const u64 {
    let hll = HyperLogLog::new(error_rate);
    let hll = Box::new(hll);
    unsafe { transmute(&*hll) }
}

#[no_mangle]
pub extern "C" fn hll_new_from_keys(error_rate: f64, key0: u64, key1: u64) -> *const u64 {
    let hll = HyperLogLog::new_from_keys(error_rate, key0, key1);
    let hll = Box::new(hll);
    unsafe { transmute(&*hll) }
}

macro_rules! hll_insert {
    ($func: ident, $ty: ty) => {
        #[no_mangle]
        pub extern "C" fn $func(hll: *mut HyperLogLog, value: $ty) {
            let hll = unsafe { &mut *hll };
            hll.insert(&value);
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
pub extern "C" fn hll_insert_str(hll: *mut HyperLogLog, value: *const c_char) {
    let hll = unsafe { &mut *hll };

    let value = unsafe {
        assert!(!value.is_null());
        std::ffi::CStr::from_ptr(value)
    };

    hll.insert(&value);
}

#[no_mangle]
pub extern "C" fn hll_len(hll: *const HyperLogLog) -> f64 {
    let hll = unsafe { &*hll };
    hll.len()
}

#[no_mangle]
pub extern "C" fn hll_drop(hll: *const HyperLogLog) {
    let hll = unsafe { &*hll };
    drop(hll);
}
