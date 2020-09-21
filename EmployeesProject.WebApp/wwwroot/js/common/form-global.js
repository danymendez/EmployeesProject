
$('form').on('submit', function (e) {
    e.preventDefault();
    let action = $(this).attr('action');
    let method = $(this).attr('method');
    let formData = new FormData(this);
    if ($('form').valid()) {
        var formsUtil = new FormUtils(action, formData, method);
        formsUtil.send();
    }

});

function FormUtils(action, formData, method) {
    this.action = action;
    this.formData = formData;
    this.method = method;
}

FormUtils.prototype.send = function () {
    switch (this.method) {
        case 'post':
            this.post();
            break;

        default:
    }
}

FormUtils.prototype.post = function () {
    var xhr = new XMLHttpRequest();
    var fom = this;
    // Open the connection
    xhr.open(this.method, this.action, true);

    // Set up a handler for when the task for the request is complete
    xhr.onload = function () {
        if (xhr.status == 200) {
            alert('se guardó correctamente');
            $('#exampleModal').closest('.modal').modal('hide');
            $('#fileModal').closest('.modal').modal('hide');
            instanceTable.ajax.reload();
            reset();
        } else {
            alert('error');
            console.error(xhr.response);
        }
    };

    // Send the data.
    xhr.send(this.formData);
}

$(function () {

     
        var attrToHide = $('[data-import]').data('import');
        $('#'+attrToHide).hide();


    $('[data-import]').on('click', function () {
        var toFireClick = $(this).data('import');
        $('#' + toFireClick).trigger("click");
    });
});

function checkfile(sender) {
    var validExts = new Array(".xlsx", ".xls");
    var fileExt = sender.value;
    fileExt = fileExt.substring(fileExt.lastIndexOf('.'));
    if (validExts.indexOf(fileExt) < 0) {
        alert("Invalid file selected, valid files are of " +
            validExts.toString() + " types.");
        
        return false;
    }
    else return true;
}