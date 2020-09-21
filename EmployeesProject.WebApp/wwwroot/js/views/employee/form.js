var instanceTable;
$(document).ready(function () {
   /// $('#employees-table').DataTable();
    $('.datepicker').datepicker({
        language: "es-ES",
        format: "dd/mm/yyyy"
    });
    var uri = $('form').attr('action').replace('Form', 'JsonForm') + '?transaction=GetAll';
    var token = $('input[name="__RequestVerificationToken"]').val();
    instanceTable= $('#employees-table').DataTable({
      
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
            "targets": 7,
            "searchable": false,
            "mRender": function (data, type, full) {
               
                return '<button type="button" class="btn btn-info" onclick="modificar(' + full.id + ');">Modificar</button><button type="button" onclick="eliminar(' + full.id +')" class="btn btn-danger">Eliminar</button>';
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
           
            var obj = JSON.parse(xhr.response);
            $('#Employee_Nombres').val(obj.employee.nombres);
            $('#Employee_Apellidos').val(obj.employee.apellidos);
            $('#Employee_FechaNacimiento').val(obj.employee.fechaNacimiento);
            $('#Employee_DUI').val(obj.employee.dui);   
            $('#Employee_NIT').val(obj.employee.nit);
            $('#Employee_ISSS').val(obj.employee.isss);
            $('#Employee_Telefono').val(obj.employee.telefono);
            $('#formModal').trigger('click');
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
       
            instanceTable.ajax.reload();
         
        } else {
            alert('error');
            console.error(xhr.response);
        }
    };

    // Send the data.
    xhr.send(this.formData);
}

function reset() {
    document.getElementById("formularioEmployee").reset();
}

function hidemodal() {
   
    $('#exampleModal').modal('hide');
    reset();
}