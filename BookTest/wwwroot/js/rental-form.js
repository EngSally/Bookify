
var selectedBookCopiesForRental = [];
$(document).ready(function () {
    $('#searchbuttom').on('click', function (e) {
        e.preventDefault();

        var serial = $('#searchinput').val();

        if (selectedBookCopiesForRental.find(c => c.serial == serial)) {
            showErrorMessage('You cannot add the same copy');
            return;
        }

        if (selectedBookCopiesForRental.length >= maxAllowedCopies) {
            showErrorMessage(`You cannot add more that ${maxAllowedCopies} book(s)`);
            return;
        }

        $('#searchForm').submit();
    });


    $('body').delegate('.js-remove', 'click', function () {
        console.log('ffffffffff');
        $(this).parents('.js-copy-container').remove();
        prepareInput();
        if ($.isEmptyObject(selectedBookCopies)) {
           
            $('#CopiesForm').find(':submit').addClass('btn-secondary');
            $('#CopiesForm').find(':submit').removeClass('btn-primary');
            $('#CopiesForm').find(':submit').attr("disabled", true);
            console.log()
        }
    });
});




function OnSerachSuccess(copy) {
    $('#searchinput').val('');

    var bookId = $(copy).find('.js-copy').data('book-id');

    if (selectedBookCopiesForRental.find(c => c.bookId == bookId)) {
        showErrorMessage('You cannot add more than one copy for the same book');
        return;
    }
    $('#CopiesForm').prepend(copy);
    $('#CopiesForm').find(':submit').disabled = false;
    $('#CopiesForm').find(':submit').removeClass('btn-secondary');
    $('#CopiesForm').find(':submit').addClass('btn-primary');
    prepareInput();
    console.log(selectedBookCopiesForRental);

}

function prepareInput() {
    selectedBookCopiesForRental = []
    var bookCopies = $('.js-copy');
    $.each(bookCopies, function (i, htmlInput) {
        var objectInput = $(htmlInput);
        selectedBookCopiesForRental.push({ serial: objectInput.val(), bookId: objectInput.data('book-id') });// SerialBookNum
        objectInput.attr('name', `SelectedCopies[${i}]`).attr('id', `SelectedCopies_${i}_`);
    });
    
}