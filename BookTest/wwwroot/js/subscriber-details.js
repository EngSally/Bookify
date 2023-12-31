﻿$(document).ready(function () {
    $('.js-renew').on('click', function () {
        var subscriberKey = $(this).data('key');
            bootbox.confirm({
                message: 'Are You Sure You Want To Renewal this Subscriber?',
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
                    console.log(`/Subscribers/RenewalSubscribtion?key=${subscriberKey}`)
                    if (result) {
                        $.post({
                            url: `/Subscribers/RenewalSubscribtion?key=${subscriberKey}`,
                            data: {
                                '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                            },
                            success: function (row) {
                                $('#SubscriptionsTable').find('tbody').append(row);
                                var activeIcon = $('#ActiveStatusIcon');
                                activeIcon.removeClass('d-none');
                                activeIcon.siblings('svg').remove();
                                activeIcon.parents('.card').removeClass('bg-warning').addClass('bg-success');
                               $('#RentalButton').removeClass('d-none');
                                $('#CardStatus').text('Active subscriber');
                                $('#StatusBadge').removeClass('badge-light-warning').addClass('badge-light-success').text('Active subscriber');
                                ShowToastrMessageSuccess('Saved Successful!');
                            },
                            error: function () {
                                ShowToastrMessagError('Error');
                            }
                        });
                    }
                }
            });
    });


    $('.js-cancel-rental').on('click', function () {
        var btn = $(this);
        bootbox.confirm({
            message: 'Are You Sure You Want To Cancel this Rental?',
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-secondary'
                }
            },

            callback: function (result) {
                console.log(`/Rentals/CancelRental/${btn.data('id')}`)
                if (result) {
                    $.post({
                        url: `/Rentals/CancelRental/${btn.data('id') }`,
                        data: {
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        success: function (data) {
                            btn.parents('tr').remove();
                            if ($('#rentalTable  tbody tr').length === 0) {
                                $('#rentalTable').fadeOut();
                                $('#RentalAlert').fadeIn();
                            }
                            var cancelNum = btn.parents('tr').find('#numofrental').data('num')
                            
                            $('#numRental').text( parseInt( $('#numRental').text()) - cancelNum)
                           
                        },
                        error: function () {
                            ShowToastrMessagError('Error');
                        }
                    });
                }
            }
        });
    });
});