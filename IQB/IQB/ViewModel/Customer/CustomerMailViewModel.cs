using IQB.EntityLayer.Common;
using IQB.EntityLayer.CustomerModels;
using IQB.IQBServices.AdminServices;
using IQB.ViewModel.Base;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;
using IQB.ViewModel.AuthViewModel;

namespace IQB.ViewModel.Customer
{
    public class CustomerMailViewModel : BaseViewModel
    {
        CustomerServices _customerServices = new CustomerServices();
        private readonly List<ListCustomerID> _customerID = new List<ListCustomerID>();
        string msg = string.Empty;
        int salonId = 0;
        ApiResult _apiresult = new ApiResult();
        public CustomerMailViewModel(List<ListCustomerID> CustomerID)
        {
            IsMailEnabled = true;
            _customerID = CustomerID;
        }

        public Command SendMailCommand
        {

            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        IsProfileApiRunning = true;
                        // Device.BeginInvokeOnMainThread(async () => {
                        var _msgresult = await App.Current.MainPage.DisplayAlert("Warning", "This will also be sent as Push Notification.Are you sure you want to send this message ? ", "Ok", "Cancel");
                        if (_msgresult)
                        {
                            IsMailEnabled = false;
                            using (SalonViewModel objSalon = new SalonViewModel())
                            {
                                salonId = objSalon.GetSalonId();
                            }
                            //var a = await EvaluateJavascript("document.getElementById('test').value;");
                            var _raweditortext = await EvaluateJavascript("tinyMCE.activeEditor.getContent();");
                            //var c= await EvaluateJavascript("tinyMCE.activeEditor.getContent({format : 'raw'});");
                            //var ce = await EvaluateJavascript("tinyMCE.activeEditor.getContent({format : 'html'});");
                            //var d= await EvaluateJavascript("tinyMCE.get('test').getContent();");
                            //var e = await EvaluateJavascript("window.parent.tinymce.get('test').getContent();");
                            //var _ConvertFromUnicode= b.Replace()


                            var _getreplacetext2 = _raweditortext.Replace("\\u003C", "<");
                            var _getreplacetext3 = _getreplacetext2.Replace("\\n", "<br/>");

                            string _editorvalue = _getreplacetext3;
                            string _editorvalue1 = _editorvalue.Trim('"');


                            CustomerMailModel _CustomerMailModel = new CustomerMailModel();
                            _CustomerMailModel.CustomerId = new List<CustomerIdModel>();

                            if (_customerID.Count >= 1)
                            {
                                if (Subject != null)
                                {
                                    if (_editorvalue1 != "")
                                    {

                                        _CustomerMailModel.MailSubject = Subject;
                                        _CustomerMailModel.MailBody = _editorvalue1;
                                        _CustomerMailModel.SalonId = salonId;
                                        for (int i = 0; i < _customerID.Count; i++)
                                        {
                                            CustomerIdModel _customerIdModel = new CustomerIdModel();
                                            _customerIdModel.CustomerID = _customerID[i].CustomerID;
                                            _CustomerMailModel.CustomerId.Add(_customerIdModel);
                                        }
                                        _apiresult = await _customerServices.PostCustomerMailForAllCustomer(_CustomerMailModel);

                                        msg = _apiresult.StatusMessage;
                                        if (_apiresult.StatusCode == 200)
                                        {

                                            await App.Current.MainPage.DisplayAlert("Sucess", msg, "Ok");
                                            Application.Current.Properties["MailSent"] = "true";
                                        }
                                        else
                                        {
                                            await App.Current.MainPage.DisplayAlert("Alert", msg, "Ok");
                                        }
                                    }
                                    else
                                    {
                                        await App.Current.MainPage.DisplayAlert("Alert", "Mail body can not be empty", "Ok");
                                    }
                                }
                                else
                                {
                                    await App.Current.MainPage.DisplayAlert("Alert", "Mail subject can not be empty", "Ok");
                                }
                            }
                        }
                    }
                    catch (Exception es)
                    {

                    }
                    IsProfileApiRunning = false;
                });
            }
        }

        //public Command OpenEditorCommand
        //{
        //    get
        //    {
        //        return new Command(async () =>
        //        {
        //            TEditorResponse response = await CrossTEditor.Current.ShowTEditor(MailBody);
        //            if (!string.IsNullOrEmpty(response.HTML))
        //            {
        //                MailBody = response.HTML;
        //                BodyHtmlSource = new HtmlWebViewSource { Html = response.HTML };
        //                //BodyHtmlSource = new HtmlWebViewSource { Html = "http://www.google.com" };
        //            }
        //        });
        //    }
        //}


        //private HtmlWebViewSource _bodyHtmlSource = new HtmlWebViewSource() { Html = "<html><head><script src = 'https://cloud.tinymce.com/stable/tinymce.min.js'></script><script>tinymce.init({ selector: 'textarea',entity_encoding : 'html' });</script></head><body><textarea id='test'></textarea></body></html>" };
        //private HtmlWebViewSource _bodyHtmlSource = new HtmlWebViewSource() { Html = "<html><head><script src = 'https://cloud.tinymce.com/stable/tinymce.min.js'></script><script>tinymce.init({ selector: 'textarea',entity_encoding : 'html',branding: false });</script><style>.mce-notification-inner {display:none!important;}.mce-close{display:none!important;}</style><head><body><textarea id='test'></textarea></body></html>" };

        private HtmlWebViewSource _bodyHtmlSource = new HtmlWebViewSource() { Html = "<html><head><script src = 'https://cloud.tinymce.com/stable/tinymce.min.js'></script><script>tinymce.init({	selector: 'textarea',	entity_encoding : 'html',	branding: false, }).then(function(){	var x = document.getElementById('test_ifr'); var y = (x.contentWindow || x.contentDocument); if (y.document)y = y.document; y.body.style.backgroundColor = '#14181B';	y.body.style.color='#FFFFFF';	var ix=document.getElementById('mceu_11');	ix.style.borderWidth='0px';	ix.style.boxShadow='none';	ix.style.border='none';	document.getElementById('mceu_30').remove();	document.getElementById('mceu_27').remove();	}); </script><style>.mce-notification-inner {display:none!important;}.mce-close{display:none!important;}</style></head><body style='background-color:#0D1013'><textarea id='test'></textarea></body></html>" };


          public HtmlWebViewSource BodyHtmlSource
        {
            get { return _bodyHtmlSource; }
            set
            {
                _bodyHtmlSource = value;
                OnPropertyChanged(nameof(BodyHtmlSource));
            }
        }

        private bool m_IsProfileApiRunning;

        public bool IsProfileApiRunning
        {
            get { return m_IsProfileApiRunning; }
            set { m_IsProfileApiRunning = value; OnPropertyChanged("IsProfileApiRunning"); }
        }

        private Func<string, Task<string>> _evaluateJavascript;
        public Func<string, Task<string>> EvaluateJavascript
        {
            get { return _evaluateJavascript; }
            set { _evaluateJavascript = value; }
        }

        private string _Subject;

        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; OnPropertyChanged("Subject"); }
        }

        private bool _IsMailEnabled;

        public bool IsMailEnabled
        {
            get { return _IsMailEnabled; }
            set { _IsMailEnabled = value; OnPropertyChanged("IsMailEnabled"); }
        }

        private string _MailBody;

        public string MailBody
        {
            get { return _MailBody; }
            set { _MailBody = value; OnPropertyChanged("MailBody"); }
        }

        private int _customerId;

        public int customerId
        {
            get { return _customerId; }
            set { _customerId = value; OnPropertyChanged("customerId"); }
        }

        private string _noofcustomer;

        public string noofcustomer
        {
            get { return _noofcustomer; }
            set { _noofcustomer = value; OnPropertyChanged("noofcustomer"); }
        }
    }
}
