var AccordianIDPrefix: string = 'accord';
jQuery(function () {
    jQuery("#accordion").accordion({ collapsible: true }).sortable({ update: SortList, forcePlaceholderSize: true });
    UpdateTasks();
});
function PutTask() {
    var name = jQuery("#name").val();
    var detail = jQuery("#details").val();
    var u = { Name: name, Details: detail };

    jQuery.ajax({
        type: "POST", url: "/api/Task/PostTask", data: u, success: PutTaskSuccess, error: PutTaskFail
    });
}
function PutTaskSuccess() {
    UpdateTasks();
}
function PutTaskFail(failure) {
    window.alert("Fail");
}
function UpdateTasks() {
    jQuery.getJSON("/api/Task/GetTask/", function (data: any[]) {

        jQuery("#accordion").empty();
        for (var x = 0; x < data.length; x++) {
            jQuery("#accordion").append("<h3 task='" + data[x].TaskID + "'>" + data[x].Name + "</h3><div class='taskdiv' task='" + data[x].TaskID + "'>" + data[x].Details + "<input id='" + data[x].TaskID + "' class='donebutton' type='button' value='Done' /></div>");
        }
        jQuery("#accordion").append('<h3>New</h3><div>Name<input id="name" type="text" /><br />Details <textarea id="details" rows="2" cols="20"></textarea><br /><input id="puttask" type="button" value="Create Task" /></div>');
        jQuery("#puttask").click(PutTask);
        jQuery("#accordion").accordion("refresh");
        jQuery(".donebutton").click(TaskCompleted);
    });
}
function TaskCompleted(event) {
    jQuery.getJSON("/api/Task/Done/" + event.target.parentElement.getAttribute("task"), function (data: any[]) {
        UpdateTasks();
    });
}
function SortList(event, ui) {
    var data = { TaskID: ui.item.attr("task"), Priority: ui.item.index()/2 };
    jQuery.ajax({
        type: "POST", url: "/api/Task/PostSort", data: data, success: PutTaskSuccess, error: PutTaskFail
    });
}