use probably::quantile::greenwald_khanna::Stream;

#[no_mangle]
pub extern "C" fn quantile_gk_new(epsilon: f64) -> *mut Stream<f64> {
    let s = Stream::new(epsilon);
    let s = Box::new(s);
    Box::leak(s)
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
