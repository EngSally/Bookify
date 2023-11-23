
var updateRow;
var table;
var datatable;
var exportedColumns = [];

function ShowToastrMessageSuccess(Message) {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toastr-bottom-left",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    toastr.success('Success');

}

function ShowToastrMessagError(Message) {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toastr-bottom-left",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };


    toastr.error('Error');

}

function DisabledSumitButton(btn) {
    $(btn).attr('disabled', 'disabled').attr('data-kt-indicator', 'on');

}

function OnmodelBegin() {
    DisabledSumitButton($('#staticBackdrop').find(':submit'));
}
function showErrorMessage(message = 'Something went wrong!') {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: message.responseText !== undefined ? message.responseText : message,
        customClass: {
            confirmButton: "btn btn-primary"
        }
    });
}

function ApplySelect2() {
    $('.js-select2').select2();
    $('.js-select2').on('select2:select', function (e) {
        $('form').not('#signOutForm').validate().element('#' + $(this).attr('id'));
    });
}

function OnmodelSuccess(row) {
    ShowToastrMessageSuccess();
    $('#staticBackdrop').modal('hide');
    var newRow = $(row)
    datatable.row.add(newRow).draw();
    if (updateRow !== undefined) {
        updateRow.addClass('animate__animated animate__flash');
        datatable.row(updateRow).remove().draw();
        updateRow = undefined;
      
    }
    

    KTMenu.init();
    KTMenu.initHandlers();
    //KTMenu.initGlobalHandlers();

}




function OnmodelComplete() {
    $('body :submit').removeAttr('disabled').removeAttr('data-kt-indicator');
}

/////Data Table


var headers = $('th');
$.each(headers, function (i) {
   
    var col = $(this);
    if (!col.hasClass('js-no-export')) {
        exportedColumns.push(i);
    }
    
});
var KTDatatables = function () {
    // Shared variables
    

    // Private functions
    var initDatatable = function () {


        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatable = $(table).DataTable({
            "info": false,
            'pageLength': 10,
            'drawCallback': function () {
                KTMenu.createInstances();
            }
        });
    }

    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = $('.js-datatables').data('export-titel');
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedColumns
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedColumns
                    }
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedColumns
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedColumns
                    },
                    customize: function (doc) {
                        pdfMake.fonts = {
                            Arial: {
                                normal: 'arial',
                                bold: 'arial',
                                italics: 'arial',
                                bolditalics: 'arial'
                            }
                        }
                        doc.defaultStyle.font = 'Arial';
                    }
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('.js-datatables');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();

///DataTable 

$(document).ready(function () {

    $('form').not('#signOutForm').on('submit', function () {

        if ($('.js-tinymce').length > 0) {
            $('.js-tinymce').each(function () {
                var input = $(this);
                var content = tinymce.get(input.attr('id')).getContent();
                input.val(content);
            });
        }
        var isValid = $(this).valid();
        if (isValid) DisabledSumitButton($(this).find(':submit'));
    });

    KTUtil.onDOMContentLoaded(function () {
        KTDatatables.init();
    });

    /////////////////////////////////Handel Bootsrap Model
    $('body').
        delegate('.js-renderPopup',
            'click',
            function () {
                var btn = $(this);
                var Modalpopup = $('#staticBackdrop');
                Modalpopup.find('#ModalTitel').text(btn.data('titel'));
                if (btn.data('update')) {
                    updateRow = btn.parents('tr');
                }

                $.get({
                    url: btn.data('url'),
                    success: function (form) {

                       
                        Modalpopup.find('#modal-body').html(form);
                        $.validator.unobtrusive.parse(Modalpopup);
                        ApplySelect2();

                    },
                    error: function () { ShowToastrMessagError('Error') }

                });


                Modalpopup.modal('show');

            }
    );
    ///
    if ($('.js-tinymce').length>0) { 
        var options = {
            selector: ".js-tinymce",
            height: "422",
           
        };
            if (KTThemeMode.getMode() === "dark") {
                options["skin"] = "oxide-dark";
                options["content_css"] = "dark";
            }

       
       

        tinymce.init(options);
        
    }


    ////Select2

    ApplySelect2();


    

    ////
    $('.js-datepicker').daterangepicker({
        singleDatePicker: true,
        autoApply: true,
        drops: 'up',
        maxDate: new Date()
    });

    /////Change Status

    $('body').delegate('.js-changstatus', 'click', function () {
        var btn = $(this)
        bootbox.confirm({
            message: 'Are You Sure You Want To Change The Status?',
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn btn-primary'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },

            callback: function (result) {

                if (result) {
                    $.post({
                        url: btn.data('url'),
                        data: {
                            '__RequestVerificationToken': $('.js-token').val()
                        },
                        success: function (updatedDate) {
                            var row = btn.parents('tr');
                            row.removeClass('animate__animated animate__flash');
                            var oldstatus = row.find('.js-status').text();
                            var newstatus = oldstatus === 'Deleted' ? 'Available' : 'Deleted'
                            btn.parents('tr').find('.js-status').text(newstatus).toggleClass(' badge-light-danger   badge-light-success');
                            btn.parents('tr').find('.js-lastupdate').text(updatedDate);

                            row.addClass('animate__animated animate__flash');

                            ShowToastrMessageSuccess('Saved Successful!');
                        }
                    });
                }
            }
        });

    });

    ///confirem Message
    $('body').delegate('.js-confirm', 'click', function () {
        var btn = $(this)
        bootbox.confirm({
            message: btn.data('message'),
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-secondary'
                }
            },

            callback: function (result) {

                if (result) {
                    $.post({
                        url: btn.data('url'),
                        data: {
                            '__RequestVerificationToken': $('.js-token').val()
                        },
                        success: function (updatedDate) {
                            var row = btn.parents('tr');
                            row.removeClass('animate__animated animate__flash');
                            var oldstatus = row.find('.js-status').text();
                            var newstatus = oldstatus === 'Deleted' ? 'Available' : 'Deleted'
                            btn.parents('tr').find('.js-status').text(newstatus).toggleClass(' badge-light-danger   badge-light-success');
                            btn.parents('tr').find('.js-lastupdate').text(updatedDate);

                            row.addClass('animate__animated animate__flash');

                            ShowToastrMessageSuccess('Successful!');




                        }


                    });
                }
                ///////////////////////////


            }
        });













    });



    $('.js-signoutlink').on('click', function () {
        $('#signOutForm').submit();
    });

});