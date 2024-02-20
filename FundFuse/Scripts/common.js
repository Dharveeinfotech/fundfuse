var _disableCopyContent = false;
$(document).ready(function () {
    if (_disableCopyContent) {
        /*Disable content selection...*/
        $("body").addClass("no_Select");
        $("body").bind("selectstart dragstart", function (e) { e.preventDefault(); return false; });
        /*Disable cut copy paste...*/
        //$("body").bind('cut copy paste', function (e) { e.preventDefault(); });
        /*Disable mouse right click...*/
        $("body").on("contextmenu", function (e) { return false; });
    }
});