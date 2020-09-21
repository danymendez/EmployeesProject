$(document).ready(function () {
   /// $('#employees-table').DataTable();
  
    var uri = $('form').attr('action').replace('Form', 'JsonForm') + '?transaction=GetAll';
    var token = $('input[name="__RequestVerificationToken"]').val();
    $('#employees-table').DataTable({
      
        destroy: true,
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.19/i18n/Spanish.json"
        },
        // "processing": "false",
        "ajax": {
            "url":uri,
            "datatype": "json",
            'beforeSend': function (request) {
                request.setRequestHeader("RequestVerificationToken", token);
            },
            "contentType": "application/json",
            "data": function (d) {
               // return JSON.stringify(datos);
            },
            "type": "Get",
            "dataSrc": "employees"
        },
        "columns": [

            { "data": "nombres" },
            { "data": "apellidos" },
            { "data": "fechaNacimiento" },
            { "data": "dui" },
            { "data": "nit" },
            { "data": "isss" },
            { "data": "telefono" }
        ],
        "columnDefs": [{
            
            "mRender": function (data, type, full) {
               
                return '<div></div>';
            }
        },
            {
                "aTargets": [2],
                "mData": "fechaNacimiento",
                "mRender": function (data, type, full) {
                    if (data === null) { return ''; } else {
                        return moment(data).format('DD/MM/YYYY');
                    }
                }
            }
        ]
    });
});

function exportToExcel() {

}

function modificar(id) {
    var xhr = new XMLHttpRequest();
    $('#Transaction').val('Update');
    $('#Employee_Id').val(id);
    // Open the connection
    xhr.open('get', $('form').attr('action').replace('Form', 'JsonForm') +'?transaction=Get&&id='+id, true);

    // Set up a handler for when the task for the request is complete
    xhr.onload = function () {
        if (xhr.status == 200) {
            alert('sucess');
            console.log(xhr.response);
            var obj = JSON.parse(xhr.response);
            $('#Employee_Nombres').val(obj.employee.nombres);
            $('#Employee_Apellidos').val(obj.employee.apellidos);
            $('#Employee_FechaNacimiento').val(obj.employee.fechaNacimiento);
            $('#Employee_DUI').val(obj.employee.dui);   
            $('#Employee_NIT').val(obj.employee.nit);
            $('#Employee_ISSS').val(obj.employee.isss);
            $('#Employee_Telefono').val(obj.employee.telefono);
        } else {
            alert('error');
            console.error(xhr.response);
        }
    };

    // Send the data.
    xhr.send(this.formData);
}


function eliminar(id) {
    var xhr = new XMLHttpRequest();
   
    // Open the connection
    xhr.open('get', $('form').attr('action').replace('Form', 'Delete') + '?id=' + id, true);

    // Set up a handler for when the task for the request is complete
    xhr.onload = function () {
        if (xhr.status == 200) {
            alert('el registro fue eliminado');
            console.log(xhr.response);
         
        } else {
            alert('error');
            console.error(xhr.response);
        }
    };

    // Send the data.
    xhr.send(this.formData);
}

