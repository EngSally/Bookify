function OnAddBookCopy(row) {
    ShowToastrMessageSuccess();
    $('#staticBackdrop').modal('hide');
    $('tbody').prepend(row);
    KTMenu.createInstances();
    var copyCount = $('#copyCount');
    var newCount = parseInt(copyCount.text()) + 1;
    copyCount.text(newCount);

    $('.js-alertNoCopy').addClass('d-none');
    $('table').removeClass('d-none');


}
function OnEditBookCopy(row) {
    ShowToastrMessageSuccess();
    $('#staticBackdrop').modal('hide');
    $(updateRow).addClass('animate__animated animate__flash');
    $(updateRow).replaceWith(row);

    KTMenu.createInstances();

}