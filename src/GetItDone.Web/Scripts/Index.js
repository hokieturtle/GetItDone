jQuery(function () {
    jQuery("#accordion").accordion({ collapsible: true });
    UpdateTasks();
});
function PutTask() {
    var name = jQuery("#name").val();
    var detail = jQuery("#details").val();
    var u = { Name: name, Details: detail };

    jQuery.ajax({
        type: "POST", url: "http://localhost:22088/api/Task/1", data: u, success: PutTaskSuccess, error: PutTaskFail
    });
}
function PutTaskSuccess() {
    window.alert("Success");
    UpdateTasks();
}
function PutTaskFail(failure) {
    window.alert("Fail");
}
function UpdateTasks() {
    jQuery.getJSON("http://localhost:22088/api/Task/1", function (data) {
        jQuery("#accordion").empty();
        for (var x = 0; x < data.length; x++) {
            jQuery("#accordion").append("<h3>" + data[x].Name + "</h3><div>" + data[x].Details + "</div>");
        }
        jQuery("#accordion").append('<h3>New</h3><div>Name<input id="name" type="text" /><br />Details <textarea id="details" rows="2" cols="20"></textarea><br /><input id="puttask" type="button" value="Create Task" /></div>');
        jQuery("#puttask").click(PutTask);
        jQuery("#accordion").accordion("refresh");
    });
}
//# sourceMappingURL=Index.js.map
