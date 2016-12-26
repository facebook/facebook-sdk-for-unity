var FacebookHelperPlugin = {
  FacebookLogInCaptureClick: function(_permissions, _callbackId) {
	var permissions = Pointer_stringify(_permissions);
	var callbackId = Pointer_stringify(_callbackId);
	console.log("Logging Facebook with permissions " + permissions);
    var OpenFacebookLoginPopup = function() {
		FBUnity.login(permissions, callbackId);
		document.getElementById('canvas').removeEventListener('click', OpenFacebookLoginPopup);
    };
    document.getElementById('canvas').addEventListener('click', OpenFacebookLoginPopup, false);
  }
};
mergeInto(LibraryManager.library, FacebookHelperPlugin);