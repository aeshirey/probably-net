use std::{ops::AddAssign, slice};

use probably::quantile::greenwald_khanna::Stream;

#[no_mangle]
pub extern "C" fn quantile_gk_new(epsilon: f64) -> *mut Stream<f64> {
    let s = Stream::new(epsilon);
    let s = Box::new(s);
    Box::leak(s)
}

#[no_mangle]
pub extern "C" fn quantile_gk_new_from_bytes(bytes: *mut u8, len: usize) -> *mut Stream<f64> {
    assert!(len > 0);
    let bytes = unsafe { slice::from_raw_parts(bytes, len) };

    let s = rmp_serde::from_slice(bytes).unwrap();
    let s = Box::new(s);
    Box::into_raw(s)
}

#[no_mangle]
pub extern "C" fn quantile_gk_merge(gk1: *mut Stream<f64>, gk2: *mut Stream<f64>) {
    let mut s1 = unsafe { Box::from_raw(gk1) };
    let s2 = unsafe { Box::from_raw(gk2) };

    // Both streams are boxed, and we can't += the boxed versions.
    // Instead, clone RHS and call .add_assign explicitly
    let s2clone = *s2.clone();
    s1.add_assign(s2clone);

    Box::leak(s1);
    Box::leak(s2);
}

#[no_mangle]
pub extern "C" fn quantile_gk_insert(s: *mut Stream<f64>, f: f64) {
    let mut s = unsafe { Box::from_raw(s) };
    s.insert(f);
    Box::leak(s);
}

#[no_mangle]
pub extern "C" fn quantile_gk_quantile(s: *mut Stream<f64>, quantile: f64) -> f64 {
    let s = unsafe { Box::from_raw(s) };
    let q = *s.quantile(quantile);
    Box::leak(s);
    q
}

#[no_mangle]
pub extern "C" fn quantile_gk_drop(s: *mut Stream<f64>) {
    let s = unsafe { Box::from_raw(s) };
    std::mem::drop(s);
}

#[no_mangle]
pub extern "C" fn quantile_gk_get_bytes(
    s: *mut Stream<f64>,
    len: &mut usize,
    cap: &mut usize,
) -> *mut u8 {
    assert!(!s.is_null());
    let s = unsafe { Box::from_raw(s) };

    let mut serialized = if let Ok(serialized) = rmp_serde::to_vec(&s) {
        serialized
    } else {
        Box::leak(s);
        return std::ptr::null_mut();
    };

    *len = serialized.len();
    *cap = serialized.capacity();
    let ptr = serialized.as_mut_ptr();

    std::mem::forget(serialized);
    Box::leak(s);
    ptr
}

#[no_mangle]
pub extern "C" fn quantile_gk_free_bytes(bytes: *mut u8, len: usize, cap: usize) {
    assert!(!bytes.is_null());
    assert!(len > 0);
    assert!(cap > 0);

    let serialized = unsafe { Vec::from_raw_parts(bytes, len, cap) };

    // Manually drop for fun.
    drop(serialized);
}
