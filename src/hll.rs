use libc::c_char;
use probably::frequency::HyperLogLog;
use std::slice;

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

#[no_mangle]
pub extern "C" fn hll_new_from_bytes(bytes: *const u8, len: usize) -> *mut HyperLogLog {
    assert!(!bytes.is_null());
    assert!(len > 0);

    let bytes = unsafe { slice::from_raw_parts(bytes, len) };

    let hll = rmp_serde::from_slice(bytes).unwrap();
    let hll = Box::new(hll);
    Box::into_raw(hll)
}

#[no_mangle]
pub extern "C" fn hll_get_bytes(
    hll: *mut HyperLogLog,
    len: &mut usize,
    cap: &mut usize,
) -> *mut u8 {
    assert!(!hll.is_null());
    let hll: Box<HyperLogLog> = unsafe { Box::from_raw(hll) };

    let mut serialized = if let Ok(serialized) = rmp_serde::to_vec(&hll) {
        serialized
    } else {
        Box::leak(hll);
        return std::ptr::null_mut();
    };

    // Set the 'out' parameters
    *len = serialized.len();
    *cap = serialized.capacity();
    let ptr = serialized.as_mut_ptr();

    // Prevent this array from being collected
    std::mem::forget(serialized);
    Box::leak(hll);
    ptr
}

#[no_mangle]
pub extern "C" fn hll_free_bytes(bytes: *mut u8, len: usize, cap: usize) {
    assert!(!bytes.is_null());
    assert!(len > 0);
    assert!(cap > 0);

    let serialized = unsafe { Vec::from_raw_parts(bytes, len, cap) };

    // Manually drop for fun.
    drop(serialized);
}

#[no_mangle]
pub extern "C" fn hll_merge(hll1: *mut HyperLogLog, hll2: *mut HyperLogLog) {
    assert!(!hll1.is_null());
    assert!(!hll2.is_null());
    let mut hll1: Box<HyperLogLog> = unsafe { Box::from_raw(hll1) };
    let hll2: Box<HyperLogLog> = unsafe { Box::from_raw(hll2) };

    hll1.merge(&hll2);

    Box::leak(hll1);
    Box::leak(hll2);
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
