var AccordianIDPrefix: string = 'accord';
jQuery(function () {
    jQuery("#accordion").accordion({ collapsible: true });
    UpdateTasks();
});
function PutTask() {
    var name = jQuery("#name").val();
    var detail = jQuery("#details").val();
    var u = { Name: name, Details: detail };

    jQuery.ajax({
        type: "POST", url: "/api/Task/", data: u, success: PutTaskSuccess, error: PutTaskFail
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
    jQuery.getJSON("/api/Task/GetTask/", function (data:any[]) {
        
        jQuery("#accordion").empty();
        for (var x = 0; x < data.length; x++) {
            jQuery("#accordion").append("<h3>" + data[x].Name + "</h3><div>" + data[x].Details + "<input id='" + data[x].TaskID+"' class='donebutton' type='button' value='Done' /></div>");
        }
        jQuery("#accordion").append('<h3>New</h3><div>Name<input id="name" type="text" /><br />Details <textarea id="details" rows="2" cols="20"></textarea><br /><input id="puttask" type="button" value="Create Task" /></div>');
        jQuery("#puttask").click(PutTask);
        jQuery("#accordion").accordion("refresh");
        jQuery(".donebutton").click(TaskCompleted);
    });
}
function TaskCompleted(event) {
    jQuery.getJSON("/api/Task/Done/"+event.target.id, function (data: any[]) {
    });
}