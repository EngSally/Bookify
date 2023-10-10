
var selectedCopies = [];
var isEditMode = false;
var currentCopies = [];
$(document).ready(function () {
  
    if($('#rental-id')[0].value !== "") {
        isEditMode = true;
        prepareInputs();
        currentCopies = selectedCopies;
    }
    


    $('#searchbuttom').on('click', function (e) {
        e.preventDefault();

        var serial = $('#searchinput').val();

        if (selectedCopies.find(c => c.serial == serial)) {
            showErrorMessage('You cannot add the same copy');
            return;
        }

        if (selectedCopies.length >= maxAllowedCopies) {
            showErrorMessage(`You cannot add more that ${maxAllowedCopies} book(s)`);
            return;
        }

        $('#searchForm').submit();
    });


    $('body').delegate('.js-remove', 'click', function () {
        var btn = $(this);
        var container = btn.parents('.js-copy-container');
        if (isEditMode) {
            btn.toggleClass('btn-light-danger btn-light-success js-remove js-readd').text('Re-Add');
            container.find('img').css('opacity', '0.5');
            container.find('h4').css('text-decoration', 'line-through');
            container.find('.js-copy').toggleClass('js-copy js-removed').removeAttr('name').removeAttr('id');
        } else {
            container.remove();
        }

        prepareInputs();

        if ($.isEmptyObject(selectedCopies) || JSON.stringify(currentCopies) == JSON.stringify(selectedCopies)) 
            $('#CopiesForm').find(':submit').addClass('d-none');
        else
            $('#CopiesForm').find(':submit').removeClass('d-none');
    });


    $('body').delegate('.js-readd', 'click', function () {
        var btn = $(this);
        var container = btn.parents('.js-copy-container');

        btn.toggleClass('btn-light-danger btn-light-success js-remove js-readd').text('Remove');
        container.find('img').css('opacity', '1');
        container.find('h4').css('text-decoration', 'none');
        container.find('.js-removed').toggleClass('js-copy js-removed');

        prepareInputs();

        if (JSON.stringify(currentCopies) == JSON.stringify(selectedCopies)) 
            $('#CopiesForm').find(':submit').addClass('d-none');
        else
            $('#CopiesForm').find(':submit').removeClass('d-none');
        
    });

});




function OnSerachSuccess(copy) {
    $('#searchinput').val('');

    var bookId = $(copy).find('.js-copy').data('book-id');

    if (selectedCopies.find(c => c.bookId == bookId)) {
        showErrorMessage('You cannot add more than one copy for the same book');
        return;
    }
    $('#CopiesForm').prepend(copy);
    $('#CopiesForm').find(':submit').removeClass('d-none');
    prepareInputs();
    console.log(selectedCopies);

}

function prepareInputs() {
    selectedCopies = []
    var bookCopies = $('.js-copy');
    $.each(bookCopies, function (i, htmlInput) {
        var objectInput = $(htmlInput);
        selectedCopies.push({ serial: objectInput.val(), bookId: objectInput.data('book-id') });// SerialBookNum
        objectInput.attr('name', `SelectedCopies[${i}]`).attr('id', `SelectedCopies_${i}_`);
    });
    
}