using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Net.Http;
using System.Text;


public enum HttpVerb
{
    GET,
    POST,
    PUT,
    DELETE
}

namespace LoaderOfCostomerData
{
    public class RestClient
    {
        private static readonly HttpClient client = new HttpClient();

        public string EndPoint { get; set; }
        public string Service { get; set; }
        public HttpVerb Method { get; set; }
        public string ContentType { get; set; }
        public string PostData { get; set; }
        public string Referer { get; set; }

        public RestClient()
        {
            EndPoint = "";
            Service = "";
            Method = HttpVerb.GET;
            ContentType = "text/xml";
            PostData = "";
            Referer = "http://kgd.gov.kz/ru/services/taxpayer_search";
        }
        public RestClient(string endpoint)
        {
            EndPoint = endpoint;
            Service = "";
            Method = HttpVerb.GET;
            ContentType = "text/xml";
            PostData = "";
            Referer = "http://kgd.gov.kz/ru/services/taxpayer_search";
        }
        public RestClient(string endpoint, HttpVerb method)
        {
            EndPoint = endpoint;
            Service = "";
            Method = method;
            ContentType = "text/xml";
            PostData = "";
            Referer = "http://kgd.gov.kz/ru/services/taxpayer_search";
        }

        public RestClient(string endpoint, HttpVerb method, string postData)
        {
            EndPoint = endpoint;
            Service = "";
            Method = method;
            ContentType = "text/xml";
            PostData = postData;
            Referer = "http://kgd.gov.kz/ru/services/taxpayer_search";
        }

        public RestClient(string endpoint, HttpVerb method, string postData, string referer)
        {
            EndPoint = endpoint;
            Service = "";
            Method = method;
            ContentType = "text/xml";
            PostData = postData;
            Referer = referer;
        }

        public RestClient(string endpoint, string service, HttpVerb method, string postData, string referer)
        {
            EndPoint = endpoint;
            Service = service;
            Method = method;
            ContentType = "text/xml";
            PostData = postData;
            Referer = referer;
        }

        public Captcha GetCaptcha(string uid)
        {
            Captcha captcha = new Captcha(uid);

            CookieContainer reqCookies = new CookieContainer();
            Cookie cookie = new Cookie();
            //Пример получения исходного кода сайта
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(EndPoint + Service);

            request1.CookieContainer = reqCookies;
            request1.Referer = Referer;

            request1.KeepAlive = true;
            request1.Method = Method.ToString();

            HttpWebResponse response = (HttpWebResponse)request1.GetResponse();

            if ((response.StatusCode == HttpStatusCode.OK ||
                 response.StatusCode == HttpStatusCode.Moved ||
                 response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {
                // if the remote file was found, download oit
                using (Stream inputStream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[16 * 1024];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        using (var capForm = new CaptchaForm(ms.ToArray()))
                        {
                            var resultCapcha = capForm.ShowDialog();
                            if (resultCapcha == DialogResult.OK)
                            {
                                captcha.TextCapcha = capForm.TextCaptcha;
                            }
                        }
                    }
                }
            }

            return captcha;
        }

        public string GetData(Captcha captcha, Request companyInfo)
        {
        CookieContainer reqCookies = new CookieContainer();
            Cookie cookie = new Cookie();
            var form_id = "";
            if (companyInfo.Type == 1)
                form_id = "taxpayer_search";
            else if (companyInfo.Type == 2)
                form_id = "taxpayer_search_legal_entity";
            else if (companyInfo.Type == 3)
                form_id = "taxpayer_search_entrepreneur";

            var request = (HttpWebRequest)WebRequest.Create(EndPoint + Service);

            var postData = "rnn=&uin=" + companyInfo.BIN + "&status=ALL&suspend_start=01.01.1995&suspend_end=" + DateTime.Now.ToString("dd.MM.yyyy") + "&name=&idCaptcha=" + captcha.IdCaptcha + "&enterCaptcha=" + captcha.TextCapcha + "&errorCaptcha=&show_print=0&time=0&result=0&form_build_id=form-Oia6-097xLhfwePdzdMOwSJ6pqU0A3wttYderTbaSk8&form_id=taxpayer_search_entrepreneur&_triggering_element_name=op&_triggering_element_value=%D0%9F%D0%BE%D0%B8%D1%81%D0%BA&ajax_html_ids%5B%5D=page&ajax_html_ids%5B%5D=CecutientWrapper&ajax_html_ids%5B%5D=panelGost&ajax_html_ids%5B%5D=WhiteStyle&ajax_html_ids%5B%5D=BlackStyle&ajax_html_ids%5B%5D=BlueStyle&ajax_html_ids%5B%5D=ImageOn&ajax_html_ids%5B%5D=ImageOff&ajax_html_ids%5B%5D=settingVer&ajax_html_ids%5B%5D=NormalVer&ajax_html_ids%5B%5D=poppedSettings&ajax_html_ids%5B%5D=c1&ajax_html_ids%5B%5D=WhiteStyle2&ajax_html_ids%5B%5D=c2&ajax_html_ids%5B%5D=BlackStyle2&ajax_html_ids%5B%5D=c3&ajax_html_ids%5B%5D=BlueStyle2&ajax_html_ids%5B%5D=c4&ajax_html_ids%5B%5D=BrownStyle&ajax_html_ids%5B%5D=c5&ajax_html_ids%5B%5D=GreenStyle&ajax_html_ids%5B%5D=headUp&ajax_html_ids%5B%5D=mobileLang&ajax_html_ids%5B%5D=caretDown&ajax_html_ids%5B%5D=loginBtn&ajax_html_ids%5B%5D=loginMobile&ajax_html_ids%5B%5D=mobiledepmenu&ajax_html_ids%5B%5D=depmenu&ajax_html_ids%5B%5D=block-smartmenus-smartmenus-2&ajax_html_ids%5B%5D=smartmenus_2&ajax_html_ids%5B%5D=sm-1498037700450051-1&ajax_html_ids%5B%5D=sm-1498037700450051-2&ajax_html_ids%5B%5D=centerUp&ajax_html_ids%5B%5D=eye-font-small&ajax_html_ids%5B%5D=eye-font-normal&ajax_html_ids%5B%5D=eye-font-big&ajax_html_ids%5B%5D=enableuGost&ajax_html_ids%5B%5D=loginBtn&ajax_html_ids%5B%5D=login&ajax_html_ids%5B%5D=masthead&ajax_html_ids%5B%5D=site-name&ajax_html_ids%5B%5D=widgetNum&ajax_html_ids%5B%5D=search&ajax_html_ids%5B%5D=block-search-solr-searchsolr&ajax_html_ids%5B%5D=search&ajax_html_ids%5B%5D=smartmenu&ajax_html_ids%5B%5D=navIco&ajax_html_ids%5B%5D=navi&ajax_html_ids%5B%5D=main-nav&ajax_html_ids%5B%5D=block-smartmenus-smartmenus-1&ajax_html_ids%5B%5D=smartmenus_1&ajax_html_ids%5B%5D=sm-14980377005006016-1&ajax_html_ids%5B%5D=sm-14980377005006016-2&ajax_html_ids%5B%5D=sm-14980377005006016-3&ajax_html_ids%5B%5D=sm-14980377005006016-4&ajax_html_ids%5B%5D=sm-14980377005006016-5&ajax_html_ids%5B%5D=sm-14980377005006016-6&ajax_html_ids%5B%5D=sm-14980377005006016-7&ajax_html_ids%5B%5D=sm-14980377005006016-8&ajax_html_ids%5B%5D=sm-14980377005006016-9&ajax_html_ids%5B%5D=sm-14980377005006016-10&ajax_html_ids%5B%5D=sm-14980377005006016-11&ajax_html_ids%5B%5D=sm-14980377005006016-12&ajax_html_ids%5B%5D=sm-14980377005006016-13&ajax_html_ids%5B%5D=sm-14980377005006016-14&ajax_html_ids%5B%5D=sm-14980377005006016-15&ajax_html_ids%5B%5D=sm-14980377005006016-16&ajax_html_ids%5B%5D=sm-14980377005006016-17&ajax_html_ids%5B%5D=sm-14980377005006016-18&ajax_html_ids%5B%5D=sm-14980377005006016-19&ajax_html_ids%5B%5D=sm-14980377005006016-20&ajax_html_ids%5B%5D=sm-14980377005006016-21&ajax_html_ids%5B%5D=sm-14980377005006016-22&ajax_html_ids%5B%5D=sm-14980377005006016-23&ajax_html_ids%5B%5D=sm-14980377005006016-24&ajax_html_ids%5B%5D=sm-14980377005006016-25&ajax_html_ids%5B%5D=sm-14980377005006016-26&ajax_html_ids%5B%5D=sm-14980377005006016-27&ajax_html_ids%5B%5D=sm-14980377005006016-28&ajax_html_ids%5B%5D=sm-14980377005006016-29&ajax_html_ids%5B%5D=sm-14980377005006016-30&ajax_html_ids%5B%5D=sm-14980377005006016-31&ajax_html_ids%5B%5D=sm-14980377005006016-32&ajax_html_ids%5B%5D=sm-14980377005006016-33&ajax_html_ids%5B%5D=sm-14980377005006016-34&ajax_html_ids%5B%5D=sm-14980377005006016-35&ajax_html_ids%5B%5D=sm-14980377005006016-36&ajax_html_ids%5B%5D=sm-14980377005006016-37&ajax_html_ids%5B%5D=sm-14980377005006016-38&ajax_html_ids%5B%5D=sm-14980377005006016-39&ajax_html_ids%5B%5D=sm-14980377005006016-40&ajax_html_ids%5B%5D=sm-14980377005006016-41&ajax_html_ids%5B%5D=sm-14980377005006016-42&ajax_html_ids%5B%5D=sm-14980377005006016-43&ajax_html_ids%5B%5D=sm-14980377005006016-44&ajax_html_ids%5B%5D=sm-14980377005006016-45&ajax_html_ids%5B%5D=sm-14980377005006016-46&ajax_html_ids%5B%5D=sm-14980377005006016-47&ajax_html_ids%5B%5D=sm-14980377005006016-48&ajax_html_ids%5B%5D=sm-14980377005006016-49&ajax_html_ids%5B%5D=sm-14980377005006016-50&ajax_html_ids%5B%5D=sm-14980377005006016-51&ajax_html_ids%5B%5D=sm-14980377005006016-52&ajax_html_ids%5B%5D=sm-14980377005006016-53&ajax_html_ids%5B%5D=sm-14980377005006016-54&ajax_html_ids%5B%5D=sm-14980377005006016-55&ajax_html_ids%5B%5D=sm-14980377005006016-56&ajax_html_ids%5B%5D=sm-14980377005006016-57&ajax_html_ids%5B%5D=sm-14980377005006016-58&ajax_html_ids%5B%5D=sm-14980377005006016-59&ajax_html_ids%5B%5D=sm-14980377005006016-60&ajax_html_ids%5B%5D=sm-14980377005006016-61&ajax_html_ids%5B%5D=sm-14980377005006016-62&ajax_html_ids%5B%5D=sm-14980377005006016-63&ajax_html_ids%5B%5D=sm-14980377005006016-64&ajax_html_ids%5B%5D=sm-14980377005006016-65&ajax_html_ids%5B%5D=sm-14980377005006016-66&ajax_html_ids%5B%5D=sm-14980377005006016-67&ajax_html_ids%5B%5D=sm-14980377005006016-68&ajax_html_ids%5B%5D=sm-14980377005006016-69&ajax_html_ids%5B%5D=sm-14980377005006016-70&ajax_html_ids%5B%5D=sm-14980377005006016-71&ajax_html_ids%5B%5D=sm-14980377005006016-72&ajax_html_ids%5B%5D=sm-14980377005006016-73&ajax_html_ids%5B%5D=sm-14980377005006016-74&ajax_html_ids%5B%5D=block-menu-block-2&ajax_html_ids%5B%5D=fixed-header&ajax_html_ids%5B%5D=breadcrumbs&ajax_html_ids%5B%5D=content&ajax_html_ids%5B%5D=container&ajax_html_ids%5B%5D=fixed-div&ajax_html_ids%5B%5D=print&ajax_html_ids%5B%5D=fixed-down&ajax_html_ids%5B%5D=content-wrap&ajax_html_ids%5B%5D=block-system-help&ajax_html_ids%5B%5D=block-system-main&ajax_html_ids%5B%5D=taxpayer-search-entrepreneur&ajax_html_ids%5B%5D=results&ajax_html_ids%5B%5D=edit-rnn&ajax_html_ids%5B%5D=edit-uin&ajax_html_ids%5B%5D=edit-status&ajax_html_ids%5B%5D=edit_status_chosen&ajax_html_ids%5B%5D=edit-suspend-start&ajax_html_ids%5B%5D=edit-suspend-end&ajax_html_ids%5B%5D=edit-name&ajax_html_ids%5B%5D=imageCaptcha&ajax_html_ids%5B%5D=reloadImg&ajax_html_ids%5B%5D=edit-entercaptcha&ajax_html_ids%5B%5D=edit-submit&ajax_html_ids%5B%5D=edit-print&ajax_html_ids%5B%5D=rightServ&ajax_html_ids%5B%5D=tabFront&ajax_html_ids%5B%5D=eservice&ajax_html_ids%5B%5D=ires&ajax_html_ids%5B%5D=tabPage&ajax_html_ids%5B%5D=blockCalen&ajax_html_ids%5B%5D=block-cctags-2&ajax_html_ids%5B%5D=myCanvas&ajax_html_ids%5B%5D=block-views-bn-block-1&ajax_html_ids%5B%5D=node-176&ajax_html_ids%5B%5D=BannerFooter&ajax_html_ids%5B%5D=prev&ajax_html_ids%5B%5D=owl-banner&ajax_html_ids%5B%5D=next&ajax_html_ids%5B%5D=colophon&ajax_html_ids%5B%5D=pin&ajax_html_ids%5B%5D=pinText&ajax_html_ids%5B%5D=downMenu&ajax_html_ids%5B%5D=block-block-10&ajax_html_ids%5B%5D=openstat1&ajax_html_ids%5B%5D=hotlog_counter&ajax_html_ids%5B%5D=hotlog_dyn&ajax_html_ids%5B%5D=_zero_63225&ajax_html_ids%5B%5D=loginFormDialog&ajax_html_ids%5B%5D=closeModal&ajax_html_ids%5B%5D=cboxOverlay&ajax_html_ids%5B%5D=colorbox&ajax_html_ids%5B%5D=cboxWrapper&ajax_html_ids%5B%5D=cboxTopLeft&ajax_html_ids%5B%5D=cboxTopCenter&ajax_html_ids%5B%5D=cboxTopRight&ajax_html_ids%5B%5D=cboxMiddleLeft&ajax_html_ids%5B%5D=cboxContent&ajax_html_ids%5B%5D=cboxTitle&ajax_html_ids%5B%5D=cboxCurrent&ajax_html_ids%5B%5D=cboxPrevious&ajax_html_ids%5B%5D=cboxNext&ajax_html_ids%5B%5D=cboxSlideshow&ajax_html_ids%5B%5D=cboxLoadingOverlay&ajax_html_ids%5B%5D=cboxLoadingGraphic&ajax_html_ids%5B%5D=cboxMiddleRight&ajax_html_ids%5B%5D=cboxBottomLeft&ajax_html_ids%5B%5D=cboxBottomCenter&ajax_html_ids%5B%5D=cboxBottomRight&ajax_html_ids%5B%5D=ui-datepicker-div&ajax_page_state%5Btheme%5D=KGD17&ajax_page_state%5Btheme_token%5D=&ajax_page_state%5Bcss%5D%5Bmodules%2Fsystem%2Fsystem.base.css%5D=1&ajax_page_state%5Bcss%5D%5Bmodules%2Fsystem%2Fsystem.menus.css%5D=1&ajax_page_state%5Bcss%5D%5Bmodules%2Fsystem%2Fsystem.messages.css%5D=1&ajax_page_state%5Bcss%5D%5Bmodules%2Fsystem%2Fsystem.theme.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Flibraries%2Fchosen%2Fchosen.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fmodules%2Fchosen%2Fcss%2Fchosen-drupal.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fmodules%2Fcalendar%2Fcss%2Fcalendar_multiday.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fmodules%2Fcolorbox_node%2Fcolorbox_node.css%5D=1&ajax_page_state%5Bcss%5D%5Bmodules%2Fcomment%2Fcomment.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fmodules%2Fdate%2Fdate_api%2Fdate.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fmodules%2Fdate%2Fdate_popup%2Fthemes%2Fdatepicker.1.7.css%5D=1&ajax_page_state%5Bcss%5D%5Bmodules%2Ffield%2Ftheme%2Ffield.css%5D=1&ajax_page_state%5Bcss%5D%5Bmodules%2Fnode%2Fnode.css%5D=1&ajax_page_state%5Bcss%5D%5Bmodules%2Fsearch%2Fsearch.css%5D=1&ajax_page_state%5Bcss%5D%5Bmodules%2Fuser%2Fuser.css%5D=1&ajax_page_state%5Bcss%5D%5Bmodules%2Fforum%2Fforum.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fmodules%2Fviews%2Fcss%2Fviews.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fmodules%2Fckeditor%2Fcss%2Fckeditor.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fcss%2Ffootable.core.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fmodules%2Fcctags%2Fcctags.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fmodules%2Fcolorbox%2Fstyles%2Fdefault%2Fcolorbox_style.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fmodules%2Fctools%2Fcss%2Fctools.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fmodules%2Fpayment_forms%2Fpayment_forms.css%5D=1&ajax_page_state%5Bcss%5D%5Bmodules%2Flocale%2Flocale.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Flibraries%2Fsmartmenus%2Fcss%2Fsm-core-css.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Flibraries%2Fsmartmenus%2Fcss%2Fsm-kgd%2Fsm-kgd.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fmodules%2Fcaptcha_is%2Fcaptcha.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fcss%2Fbootstrap.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fcss%2Fanimate.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fcss%2Ffont-awesome.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fcss%2Fsimple-line-icons.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fcss%2Fapp.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fcss%2Fflexslider.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fcss%2Fdatepicker.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fcss%2Fkgd17.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fcss%2Fowl.carousel.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fcss%2Fowl.theme.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fcss%2Fspecial.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fcss%2Fjquery.navgoco.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fcss%2Fjquery.dataTables.css%5D=1&ajax_page_state%5Bcss%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fstyle.css%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Flibraries%2Fsmartmenus%2Fjquery.smartmenus.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fmodules%2Fjquery_update%2Freplace%2Fjquery%2F1.10%2Fjquery.min.js%5D=1&ajax_page_state%5Bjs%5D%5Bmisc%2Fjquery.once.js%5D=1&ajax_page_state%5Bjs%5D%5Bmisc%2Fdrupal.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fmodules%2Fjquery_update%2Freplace%2Fui%2Fexternal%2Fjquery.cookie.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fmodules%2Fjquery_update%2Freplace%2Fmisc%2Fjquery.form.min.js%5D=1&ajax_page_state%5Bjs%5D%5Bmisc%2Fstates.js%5D=1&ajax_page_state%5Bjs%5D%5Bmisc%2Fajax.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fmodules%2Fjquery_update%2Fjs%2Fjquery_update.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Flibraries%2Fchosen%2Fchosen.jquery.min.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fjs%2Ffootable.js%5D=1&ajax_page_state%5Bjs%5D%5Bpublic%3A%2F%2Flanguages%2Fru_dwQ68aYIFuc_-XtscLZQZibyQC1o4bAYy0zl96l2guk.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fjs%2Fforum_for_mobile.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Flibraries%2Fcolorbox%2Fjquery.colorbox-min.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fmodules%2Fcolorbox%2Fjs%2Fcolorbox.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fmodules%2Fcolorbox%2Fstyles%2Fdefault%2Fcolorbox_style.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fmodules%2Fcolorbox%2Fjs%2Fcolorbox_load.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fmodules%2Fcolorbox%2Fjs%2Fcolorbox_inline.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fmodules%2Fcctags%2Fjs%2Fjquery.tagcanvas.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fmodules%2Fcctags%2Fjs%2Fj.tag.start.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fmodules%2Fgoogle_analytics%2Fgoogleanalytics.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fmodules%2Fsmartmenus%2Fjs%2Fsmartmenus.settings.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fmodules%2Fcaptcha_is%2Fcaptcha.js%5D=1&ajax_page_state%5Bjs%5D%5Bmisc%2Fprogress.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Flibraries%2Fjquery-block-ui%2Fjquery.blockui.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fmodules%2Fcolorbox_node%2Fcolorbox_node.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fmodules%2Fchosen%2Fchosen.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fjs%2Fbootstrap.min.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fjs%2Fjquery-ui.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fjs%2Fsuperfish.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fjs%2Fmobilemenu.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fjs%2Fowl.carousel.min.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fjs%2Fowl.carousel.start.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fjs%2Fcustom.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fjs%2Fjquery.navgoco.min.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fjs%2Fui.datepicker-lang.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fjs%2Fjquery.scrolltofixed.min.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fjs%2Fspecver.js%5D=1&ajax_page_state%5Bjs%5D%5Bsites%2Fall%2Fthemes%2FKGD17%2Fjs%2Ffunctions.js%5D=1&ajax_page_state%5Bjquery_version%5D=1.10";
           
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //Пример получения исходного кода сайта
            //var data = new Dictionary<string, string>
            //{
            //    { "rnn", "" },
            //    { "uin", companyInfo.BIN },
            //    { "status", "ALL" },
            //    { "suspend_start", new DateTime(1995,1,1).ToString("dd.MM.yyyy")},
            //    { "suspend_end",  DateTime.Now. ToString("dd.MM.yyyy") },
            //    { "name",  companyInfo.Name },
            //    { "idCaptcha",  captcha.IdCaptcha },
            //    { "enterCaptcha",  captcha.TextCapcha },
            //    { "errorCaptcha",  "" },
            //    { "show_print",  "0" },
            //    { "time",  "0" },
            //    { "result",  "0" },
            //    { "form_build_id",  "form-Oia6-097xLhfwePdzdMOwSJ6pqU0A3wttYderTbaSk8" },
            //    { "form_id",  form_id },
            //    { "_triggering_element_name",  "op" },
            //    { "_triggering_element_value",  "Поиск" },
            //    { "ajax_html_ids",  "{}" },
            //    { "ajax_page_state",  "{css,{modules/system/system.base.css, 1}}" }


            //};

            //var content = new FormUrlEncodedContent(data);

            //var response = client.PostAsync(EndPoint + Service, new StringContent(PostData.Insert());

            //var responseString = response.Result.Content.ReadAsStringAsync().Result;
            //HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(EndPoint + Service);

            //request1.CookieContainer = reqCookies;
            //request1.Referer = Referer;

            //request1.KeepAlive = true;
            //request1.Method = Method.ToString();

            //HttpWebResponse response = (HttpWebResponse)request1.GetResponse();

            //if ((response.StatusCode == HttpStatusCode.OK ||
            //     response.StatusCode == HttpStatusCode.Moved ||
            //     response.StatusCode == HttpStatusCode.Redirect) &&
            //    response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            //{
            //    // if the remote file was found, download oit
            //    using (Stream inputStream = response.GetResponseStream())
            //    {
            //        byte[] buffer = new byte[16 * 1024];
            //        using (MemoryStream ms = new MemoryStream())
            //        {
            //            int read;
            //            while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            //            {
            //                ms.Write(buffer, 0, read);
            //            }
            //            using (var capForm = new CaptchaForm(ms.ToArray()))
            //            {
            //                var resultCapcha = capForm.ShowDialog();
            //                if (resultCapcha == DialogResult.OK)
            //                {
            //                    captcha.TextCapcha = captcha.TextCapcha;
            //                }
            //            }
            //        }
            //    }
            //}

            return responseString;
        }

    }
}