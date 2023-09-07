$(document).ready(function () {
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
                                console.log(row);
                                $('#SubscriptionsTable').find('tbody').append(row);
                                var activeIcon = $('#ActiveStatusIcon');
                                activeIcon.removeClass('d-none');
                                activeIcon.siblings('svg').remove();
                                activeIcon.parents('.card').removeClass('bg-warning').addClass('bg-success');

                                $('#CardStatus').text('Active subscriber');
                                $('#StatusBadge').removeClass('badge-light-warning').addClass('badge-light-success').text('Active subscriber');
                                ShowToastrMessageSuccess('Saved Successful!');
                            }
                        });
                    }
                }

           });
   });
 });