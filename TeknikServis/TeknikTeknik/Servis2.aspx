<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Servis2.aspx.cs" Inherits="TeknikServis.TeknikTeknik.Servis2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";

            if (confirm("Kaydı silmek istiyor musunuz?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

    </script>
    <div class="kaydir">
        <div class="panel-group" id="accordionik">
            <div id="panelContents" runat="server" class="panel panel-info">
                <div class="panel-heading">
                    <h2 class="panel-title">
                        <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" class="collapsed">İş/Şantiye Bilgileri</a>
                    </h2>

                </div>
                <div id="collapseOne" class="panel-collapse in" style="height: auto;">
                    <div class="panel-body">

                        <div class="btn-group visible-lg pull-right">
                            <asp:Button ID="btnEkle" runat="server" CssClass="btn btn-info" Text="Detay" OnClick="btnEkle_Click" />
                            <asp:Button ID="btnSonlandir" runat="server" CssClass="btn btn-info" Text="Sonladır" OnClick="btnSonlandir_Click" />
                            <asp:Button ID="btnBelge" runat="server" CssClass="btn btn-info" Text="Belge" OnClick="btnBelge_Click" />
                            <asp:LinkButton ID="btnMusteriDetayim"
                                runat="server" Visible="false"
                                CssClass="btn btn-info " OnClick="btnMusteriDetayim_Click"
                                Text="<i class='fa fa-user icon-2x'></i>" />
                        </div>

                        <div class="btn-group visible-sm visible-xs pull-right">
                            <asp:LinkButton ID="btnEkleK"
                                runat="server"
                                CssClass="btn btn-primary " OnClick="btnEkle_Click"
                                Text="<i class='fa fa-cog icon-2x'></i>" />

                            <asp:LinkButton ID="btnSonlandirK"
                                runat="server"
                                CssClass="btn btn-success " OnClick="btnSonlandir_Click"
                                Text="<i class='fa fa-hourglass-end icon-2x'></i>" />

                            <asp:LinkButton ID="btnMusteriDetayimK"
                                runat="server" Visible="false"
                                CssClass="btn btn-info " OnClick="btnMusteriDetayim_Click"
                                Text="<i class='fa fa-user icon-2x'></i>" />

                            <asp:Button ID="btnBelgeK" runat="server" CssClass="btn btn-info" Text="Belge" OnClick="btnBelge_Click" />

                        </div>
                        <div class="col-md-12">
                            <h3>
                                <label id="txtMusteri" runat="server" class="label label-danger"></label>
                            </h3>
                            <h3 id="txtKonu" runat="server"></h3>
                            <asp:HiddenField ID="hdnAciklama" runat="server" />

                            <p id="txtServisAciklama" runat="server" class="lead"></p>

                            <p id="txtServisAdresi" runat="server" class="lead"></p>
                            <asp:HiddenField ID="hdnServisID" runat="server" />
                            <asp:HiddenField ID="hdnKapanma" runat="server" />
                        </div>
                        <div class="col-md-6">
                            <label id="lblKimlik" for="txtKimlikNo">Servis No:</label>
                            <input class="form-control alert-info" id="txtKimlikNo" runat="server" type="text" disabled="disabled" />
                            <label id="lblTarih" for="txtTarih">Servis Tarihi:</label>
                            <input class="form-control alert-warning" id="txtTarih" runat="server" type="datetime" />


                        </div>
                        <div class="col-md-6">
                            <label id="lblTutar" for="txtServisTutar">İş Tutarı:</label>
                            <input class="form-control alert-info" id="txtServisTutar" runat="server" type="text" disabled="disabled" />
                            <label id="lblKullanici" for="txtKullanici">Kullanıcı:</label>
                            <input class="form-control alert-success" id="txtKullanici" runat="server" />
                        </div>
                        <%-- Servis tutarı
                            Görevli usta
                             Cihaz adı--%>
                        <input type="hidden" name="txtServisID" value="" runat="server" id="txtServisID" />
                        <input type="hidden" name="txtAtanan" value="" runat="server" id="hdnAtananID" />
                        <input type="hidden" name="txtCust" value="" runat="server" id="hdnCustID" />
                    </div>
                </div>

            </div>

            <div id="Div2" runat="server" class="panel panel-info">
                <div class="panel-heading">
                    <h2 class="panel-title">
                        <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" class="collapsed">Malzeme/Makine Hesapları</a>
                    </h2>
                </div>
                <div id="collapseTwo" class="panel-collapse collapse" style="height: 0px;">
                    <div class="panel-body">
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                            <ProgressTemplate>
                                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <div class="table-responsive">
                            <%-- servis hesapları başlıyor --%>
                            <asp:UpdatePanel ID="upCrudGrid" runat="server" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <div id="cariOzet" runat="server" class="pull-right">
                                        <span id="txtHesapAdet" runat="server" class="label label-success"></span>
                                        <span id="txtHesapMaliyet" runat="server" class="label label-warning"></span>
                                        <span id="txtHesapTutar" runat="server" class="label label-info"></span>
                                    </div>
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                        DataKeyNames="hesapID"
                                        EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound"
                                        OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="true" PageSize="10">
                                        <PagerStyle CssClass="pagination-ys" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" HeaderStyle-Width="100">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="delLink"
                                                        runat="server"
                                                        CssClass="btn btn-danger btn-xs"
                                                        CommandName="del" CommandArgument='<%#Eval("hesapID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                                    <asp:LinkButton ID="btnDuzenle"
                                                        runat="server"
                                                        CssClass="btn btn-primary btn-xs" Visible="false"
                                                        CommandName="duzenle" CommandArgument='<%#Eval("hesapID")+ ";" + Container.DisplayIndex  %>' Text="<i class='fa fa-pencil'></i>" />

                                                    <asp:LinkButton ID="btnOnay"
                                                        runat="server"
                                                        CssClass="btn btn-success btn-xs"
                                                        CommandName="onay" CommandArgument='<%#Eval("hesapID")+ ";" + Container.DisplayIndex +";"+ Eval("MusteriID")  %>' Text="<i class='fa fa-check'></i>" />

                                                    <asp:LinkButton ID="btnMasrafGir"
                                                        runat="server"
                                                        CssClass="btn btn-primary btn-xs"
                                                        Text="<i class='fa fa-hourglass'></i>" />

                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm" ItemStyle-CssClass="visible-xs visible-sm">
                                                <ItemTemplate>


                                                    <asp:LinkButton ID="delLinkk"
                                                        runat="server"
                                                        CssClass="btn btn-danger btn-md"
                                                        CommandName="del" CommandArgument='<%#Eval("hesapID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                                    <asp:LinkButton ID="btnDuzenlek"
                                                        runat="server"
                                                        CssClass="btn btn-primary btn-md"
                                                        CommandName="duzenle" CommandArgument='<%#Eval("hesapID")+ ";" + Container.DisplayIndex  %>' Text="<i class='fa fa-pencil'></i>" />

                                                    <asp:LinkButton ID="btnOnayk"
                                                        runat="server"
                                                        CssClass="btn btn-success btn-md"
                                                        CommandName="onay" CommandArgument='<%#Eval("hesapID")+ ";" + Container.DisplayIndex +";"+ Eval("MusteriID")  %>' Text="<i class='fa fa-check'></i>" />
                                                    <asp:LinkButton ID="btnMasrafGirk"
                                                        runat="server"
                                                        CssClass="btn btn-primary btn-md"
                                                        Text="<i class='fa fa-hourglass'></i>" />
                                                </ItemTemplate>

                                            </asp:TemplateField>

                                            <asp:BoundField DataField="hesapID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                <HeaderStyle CssClass="visible-lg" />
                                                <ItemStyle CssClass="visible-lg" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="islemParca" HeaderText="İşlem" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="cihaz" HeaderText="Makine/Malzeme"></asp:BoundField>
                                            <asp:BoundField DataField="dakika" HeaderText="Miktar" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                            <asp:BoundField DataField="tarife" HeaderText="Tarife"></asp:BoundField>
                                            <asp:BoundField DataField="sure" HeaderText="Süre"></asp:BoundField>
                                            <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                            <asp:BoundField DataField="yekun" HeaderText="Yekün" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="toplam_maliyet" HeaderText="Toplam Maliyet" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                            <asp:BoundField DataField="servisID" HeaderText="SID" HeaderStyle-CssClass="hidden-xs  hidden-lg" ItemStyle-CssClass="hidden-xs hidden-lg"></asp:BoundField>
                                            <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>

                                        </Columns>

                                    </asp:GridView>

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />

                                </Triggers>
                            </asp:UpdatePanel>

                            <%-- servishesapları bitiyor --%>


                            <div id="onayModalH" class="modal  fade" tabindex="-1" role="dialog"
                                aria-labelledby="addModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-content modal-sm">

                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">

                                        <ContentTemplate>
                                            <div class="modal-body">
                                                <div class="row">

                                                    <div class="col-md-12">
                                                        <div class="alert alert-info text-center">
                                                            <i class="fa fa-2x">Servis hesabını onaylıyor musunuz?</i>
                                                            <%--<div class="checkbox-inline">
                                                                <asp:CheckBox ID="chcSmsH" Text="SMS" runat="server" />
                                                            </div>
                                                            <div class="checkbox-inline">
                                                                <asp:CheckBox ID="chcMailH" Text="Mail" runat="server" />
                                                            </div>--%>
                                                            <div class="btn-group pull-right">
                                                                <asp:Button ID="btnOnayH" runat="server" Text="Tamam"
                                                                    CssClass="btn btn-success" OnClick="btnOnay_ClickH" />
                                                                <button class="btn btn-warning" data-dismiss="modal"
                                                                    aria-hidden="true">
                                                                    Kapat</button>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <asp:HiddenField ID="hdnHesapIDH" runat="server" />
                                                    <asp:HiddenField ID="hdnMusteriIDH" runat="server" />
                                                    <asp:HiddenField ID="hdnServisIDDH" runat="server" />
                                                    <asp:HiddenField ID="hdnIslemmH" runat="server" />
                                                    <asp:HiddenField ID="hdnYekunnH" runat="server" />
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnOnayH" EventName="Click" />

                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div id="topluModal" class="modal  fade" tabindex="-1" role="dialog"
                                aria-labelledby="addModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-content modal-sm">

                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">

                                        <ContentTemplate>
                                            <div class="modal-body">
                                                <div class="row">

                                                    <div class="col-md-12">
                                                        <div class="alert alert-info text-center">
                                                            <i class="fa fa-2x">İş hesaplarının hepsini onaylıyor musunuz?</i>

                                                            <div class="btn-group pull-right">

                                                                <asp:Button ID="btnTopluOnayKaydet" runat="server" Text="Tamam"
                                                                    CssClass="btn btn-success" OnClick="btnTopluOnayKaydet_Click" />
                                                                <button class="btn btn-warning" data-dismiss="modal"
                                                                    aria-hidden="true">
                                                                    Kapat</button>

                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnTopluOnay" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnTopluOnayK" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>


                            <div id="tamirciModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="tamirciModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-content modal-lg">
                                    <div class="modal-header modal-header-info">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                        <h3 id="tamirciModalLabel" class="baslik">Al/Sat Hesabı(Örnek: Dış Kiralama)</h3>
                                    </div>

                                    <div class="modal-body">

                                        <%--  <div class="panel panel-info">
                                <div class="panel-heading">
                                    SERVİS HESAP VE KARARI
                                </div>--%>
                                        <%-- %> <div class="panel-body"> --%>

                                        <div class="panel panel-info">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseMusteri" class="collapsed">Dış Firma Seçimi</a>
                                                </h4>
                                            </div>
                                            <%-- müşteri aramayı ekleyecez --%>
                                            <div id="collapseMusteri" class="panel-collapse in" style="height: auto;">
                                                <div class="panel-body">
                                                    <!-- Müşteri seçim alanı başlıyor -->

                                                    <div class="table-responsive">
                                                        <asp:Panel runat="server" ID="panelTamirci" DefaultButton="btnMusteriAra">
                                                            <div class="input-group custom-search-form">
                                                                <input runat="server" type="text" id="txtMusteriSorgu" class="form-control" placeholder="Ara..." />
                                                                <span class="input-group-btn">
                                                                    <asp:LinkButton ID="btnMusteriAra"
                                                                        runat="server"
                                                                        CssClass="btn btn-info" OnClick="MusteriAra"
                                                                        Text="<i class='fa fa-search icon-2x'></i>" />
                                                                </span>
                                                            </div>
                                                        </asp:Panel>

                                                    </div>

                                                    <!-- Müşteri seç,malanı bitiyor-->
                                                </div>
                                            </div>
                                        </div>
                                        <asp:UpdatePanel ID="UpdatePanelKarar" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="grdMusteri" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="CustID"
                                                    EmptyDataText="Kayıt girilmemiş" EnablePersistedSelection="false"
                                                    OnSelectedIndexChanged="grdMusteri_SelectedIndexChanged" OnRowCommand="grdMusteri_RowCommand">
                                                    <SelectedRowStyle CssClass="danger" />

                                                    <Columns>
                                                        <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Seç">
                                                            <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                                                        </asp:ButtonField>
                                                        <asp:BoundField DataField="CustID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                            <HeaderStyle CssClass="visible-lg" />
                                                            <ItemStyle CssClass="visible-lg" />
                                                        </asp:BoundField>

                                                        <asp:TemplateField HeaderText="Tedarikçi Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="btnRandom"
                                                                    runat="server"
                                                                    CssClass="btn btn-primary"
                                                                    CommandName="detail" CommandArgument='<%#Eval("CustID") %>' Text=' <%#Eval("Ad") %> '>  </asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Adres" HeaderText="Adres" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                            <HeaderStyle CssClass="visible-lg" />
                                                            <ItemStyle CssClass="visible-lg" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Telefon" HeaderText="Telefon" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                            <HeaderStyle CssClass="visible-lg" />
                                                            <ItemStyle CssClass="visible-lg" />
                                                        </asp:BoundField>

                                                    </Columns>

                                                </asp:GridView>

                                                <%-- tamircinin yapacağı işlem bilgileri --%>
                                                <div class="panel panel-info">
                                                    <div class="panel-heading">
                                                        <h4 class="panel-title">
                                                            <a data-toggle="collapse" data-parent="#accordion" href="#collapseKarar">Al/Sat Yapılan Hizmet Bilgileri</a>
                                                        </h4>
                                                    </div>
                                                    <div id="collapseKarar" class="panel-collapse in" style="height: auto;">
                                                        <div class="panel-body">
                                                            <asp:Panel runat="server" ID="panelTamirci2" DefaultButton="btnKaydetTamirci">

                                                                <div class="form-horizontal">

                                                                    <div class="form-group">
                                                                        <label class="col-sm-2 control-label" for="txtIslemParcaTamirci">Yapılacak İşlem</label>
                                                                        <div class="col-sm-10">
                                                                            <input id="txtIslemParcaTamirci" runat="server" type="text" class="form-control" />
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" EnableClientScript="true" CssClass="text-danger" ControlToValidate="txtIslemParcaTamirci" ErrorMessage="Lütfen yapılacak işlemi giriniz" ValidationGroup="valGrupTamir"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                    <asp:HiddenField ID="hdnTamirciID" runat="server" Value="" />
                                                                    <div class="form-group">

                                                                        <label class="col-sm-2 control-label" for="txtKDVOraniDuzenleTamirci">KDV Oranı</label>
                                                                        <div class="col-sm-10">
                                                                            <asp:TextBox ID="txtKDVOraniDuzenleTamirci" runat="server" Text="0" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group" hidden>

                                                                        <label class="col-sm-2 control-label" for="txtKDVDuzenleTamirci">KDV Tutarı</label>
                                                                        <div class="col-sm-10">
                                                                            <asp:TextBox ID="txtKDVDuzenleTamirci" Enabled="false" runat="server" Text="18" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <%-- Burası müşteriye verdiğimiz fiyat --%>
                                                                        <label class="col-sm-2 control-label" for="txtYekunTamirci">Tutar(KDV Dahil)</label>
                                                                        <div class="col-sm-10">
                                                                            <asp:TextBox ID="txtYekunTamirci" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" EnableClientScript="true" ControlToValidate="txtYekunTamirci" ErrorMessage="Lütfen toplam tutar giriniz" CssClass="text-danger" ValidationGroup="valGrupTamir"></asp:RequiredFieldValidator>
                                                                            <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtYekunTamirci" Type="Currency" ValidationGroup="valGrupTamir" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>
                                                                            <asp:HiddenField ID="hdnHesapIDDuzenTamirci" runat="server" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <%-- Burası müşteriye verdiğimiz fiyat --%>
                                                                        <label class="col-sm-2 control-label" for="txtTamirciMaliyet">Maliyet(Dışarıya Ödeyeceğimiz)</label>
                                                                        <div class="col-sm-10">
                                                                            <asp:TextBox ID="txtTamirciMaliyet" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" EnableClientScript="true" ControlToValidate="txtTamirciMaliyet" ErrorMessage="Lütfen toplam tutar giriniz" CssClass="text-danger" ValidationGroup="valGrupTamir"></asp:RequiredFieldValidator>
                                                                            <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtTamirciMaliyet" Type="Currency" ValidationGroup="valGrupTamir" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>

                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">

                                                                        <label class="col-sm-2 control-label" for="txtAciklamaTamirci">Açıklama</label>
                                                                        <div class="col-sm-10">
                                                                            <asp:TextBox ID="txtAciklamaTamirci" runat="server" TextMode="MultiLine" CssClass="form-control" ValidationGroup="valGrupTamir"></asp:TextBox>
                                                                        </div>
                                                                    </div>

                                                                    <div class="form-group">

                                                                        <label for="tarihtamirci" class="col-md-2 control-label">İşlem Tarihi</label>
                                                                        <div class="col-md-10">

                                                                            <input type='text' id="tarihtamirci" runat="server" class="form-control" />

                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <div class="btn-group-justified">
                                                                            <div class="col-md-10  pull-right">
                                                                                <asp:Button ID="btnKaydetTamirci" runat="server" Text="Kaydet" CausesValidation="true" ValidationGroup="valGrupTamir" CssClass="btn btn-info btn-block" OnClick="btnKaydetTamirci_Click" />
                                                                                <button class="btn btn-primary btn-block" data-dismiss="modal" aria-hidden="true">Kapat</button>
                                                                            </div>
                                                                            <%--<div class="col-md-4 ">
                                                                          
                                                                        </div>--%>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="grdMusteri" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="btnMusteriAra" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <%--</div>--%>
                                        <%--</div>--%>

                                        <%--  <div class="modal-footer">
                                
                            </div>--%>
                                    </div>


                                </div>

                            </div>

                            <div id="yeniModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="yeniModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-content modal-lg">
                                    <div class="modal-header modal-header-info">
                                        <a class="close" data-dismiss="modal" aria-hidden="true">×</a>
                                        <h3 id="yeniModalLabel" class="baslik">Malzeme Satış Hesabı</h3>
                                    </div>

                                    <div class="modal-body">

                                        <div class="panel panel-info">
                                            <div class="panel-heading">
                                                Malzeme Satış Hesap Bilgileri
                                            </div>
                                            <div class="panel-body">
                                                <div class="panel panel-info">
                                                    <div class="panel-heading">
                                                        <h4 class="panel-title">
                                                            <a data-toggle="collapse" data-parent="#accordion" href="#collapseYeni" class="collapsed">Malzeme Seçimi</a>
                                                        </h4>
                                                    </div>
                                                    <div id="collapseYeni" class="panel-collapse collapse" style="height: 0px;">
                                                        <div class="panel-body">
                                                            <!-- Müşteri seçim alanı başlıyor -->

                                                            <div class="table-responsive">
                                                                <asp:Panel runat="server" ID="panelYeniCihaz" DefaultButton="btnARA">

                                                                    <div class="input-group custom-search-form ">
                                                                        <span class="input-group-btn">
                                                                            <asp:LinkButton ID="btnARA"
                                                                                runat="server"
                                                                                CssClass="btn btn-info" OnClick="CihazAra"
                                                                                Text="<i class='fa fa-search icon-2x'></i>" />
                                                                        </span>
                                                                        <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ara..." />

                                                                    </div>
                                                                </asp:Panel>
                                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                                    <ContentTemplate>

                                                                        <asp:GridView ID="grdCihaz" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="ID"
                                                                            EmptyDataText="Malzeme girilmemiş" OnSelectedIndexChanged="grdCihaz_SelectedIndexChanged">
                                                                            <SelectedRowStyle CssClass="danger" />
                                                                            <Columns>

                                                                                <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Seç" HeaderText="Seçim">
                                                                                    <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                                                                                </asp:ButtonField>
                                                                                <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                                    <HeaderStyle CssClass="visible-lg" />
                                                                                    <ItemStyle CssClass="visible-lg" />
                                                                                </asp:BoundField>

                                                                                <asp:TemplateField HeaderText="Malzeme Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">

                                                                                    <ItemTemplate>


                                                                                        <asp:LinkButton ID="btnRandom"
                                                                                            runat="server"
                                                                                            CssClass="btn btn-primary"
                                                                                            CommandName="detail" CommandArgument='<%#Eval("ID") %>' Text=' <%#Eval("cihaz_adi") %> '>
                           
                                                                                        </asp:LinkButton>
                                                                                    </ItemTemplate>

                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                                    <HeaderStyle CssClass="visible-lg" />
                                                                                    <ItemStyle CssClass="visible-lg" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="bakiye" HeaderText="Stok" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                                    <HeaderStyle CssClass="visible-lg" />
                                                                                    <ItemStyle CssClass="visible-lg" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="fiyat" HeaderText="Son Alış" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                                    <HeaderStyle CssClass="visible-lg" />
                                                                                    <ItemStyle CssClass="visible-lg" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="sinirsiz" HeaderText="Kaynak" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                                    <HeaderStyle CssClass="visible-lg" />
                                                                                    <ItemStyle CssClass="visible-lg" />
                                                                                </asp:BoundField>
                                                                            </Columns>

                                                                        </asp:GridView>
                                                                        <asp:Button ID="btnAddCihaz" runat="server" Text="Yeni Ürün/Parça" CssClass="btn btn-info"
                                                                            OnClick="btnAddCihaz_Click" />
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="btnARA" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="grdCihaz" EventName="RowCommand" />
                                                                        <asp:AsyncPostBackTrigger ControlID="btnEkleH" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="btnEkleHK" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="btnTamirciyeK" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="btnTamirciye" EventName="Click" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>


                                                            </div>

                                                            <!-- Müşteri seç,malanı bitiyor-->
                                                        </div>
                                                    </div>
                                                </div>

                                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                    <ContentTemplate>

                                                        <div class="panel panel-info">
                                                            <div class="panel-heading">
                                                                <h4 class="panel-title">
                                                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseKararil">Hesap Bilgileri</a>
                                                                </h4>
                                                            </div>
                                                            <div id="collapseKararil" class="panel-collapse in" style="height: auto;">
                                                                <div class="panel-body">
                                                                    <asp:Panel runat="server" ID="panelMalzeme" DefaultButton="btnKaydet">


                                                                        <div class="form-horizontal">

                                                                            <div class="form-group">
                                                                                <label class="col-sm-2 control-label" for="txtAdet">Miktar</label>
                                                                                <div class="col-sm-6">
                                                                                    <asp:TextBox ID="txtAdet" runat="server" Text="1" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtAdet_TextChanged"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" EnableClientScript="true" ControlToValidate="txtAdet" ErrorMessage="Lütfen miktar giriniz" CssClass="text-danger" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                                                                    <asp:RangeValidator ID="RangeValidator7" runat="server" ControlToValidate="txtAdet" Type="Currency" ValidationGroup="valGrup" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>

                                                                                </div>
                                                                                <div class="col-sm-2">
                                                                                    <asp:DropDownList ID="drdBirim" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">
                                                                                <label class="col-sm-2 control-label" for="txtCihazAdiGoster">Malzeme</label>
                                                                                <div class="col-sm-10">
                                                                                    <input id="txtCihazAdiGoster" runat="server" type="text" class="form-control" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">
                                                                                <label class="col-sm-2 control-label" for="txtIslemParca">Yapılacak İş</label>
                                                                                <div class="col-sm-10">
                                                                                    <input id="txtIslemParca" runat="server" type="text" class="form-control" />
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" EnableClientScript="true" CssClass="text-danger" ControlToValidate="txtIslemParca" ErrorMessage="Lütfen yapılacak işlemi giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>

                                                                            <div class="form-group">

                                                                                <label class="col-sm-2 control-label" for="txtFiyatMalzeme">Fiyat</label>
                                                                                <div class="col-sm-10">
                                                                                    <asp:TextBox ID="txtFiyatMalzeme" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtFiyatMalzeme_TextChanged"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">

                                                                                <label class="col-sm-2 control-label" for="txtKDVOraniDuzenle">KDV Oranı</label>
                                                                                <div class="col-sm-10">
                                                                                    <asp:TextBox ID="txtKDVOraniDuzenle" runat="server" Text="18" CssClass="form-control"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div hidden class="form-group">

                                                                                <label class="col-sm-2 control-label" for="txtKDVDuzenle">KDV Tutarı</label>
                                                                                <div class="col-sm-10">
                                                                                    <asp:TextBox ID="txtKDVDuzenle" Enabled="false" runat="server" Text="18" CssClass="form-control"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">

                                                                                <label class="col-sm-2 control-label" for="txtYekun">Tutar(KDV Dahil)</label>
                                                                                <div class="col-sm-10">
                                                                                    <asp:TextBox ID="txtYekun" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" EnableClientScript="true" ControlToValidate="txtYekun" ErrorMessage="Lütfen toplam tutar giriniz" CssClass="text-danger" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                                                                    <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtYekun" Type="Currency" ValidationGroup="valGrup" CssClass="text-danger" MaximumValue="99999" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>
                                                                                    <asp:HiddenField ID="hdnHesapIDDuzen" runat="server" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">

                                                                                <label class="col-sm-2 control-label" for="txtAciklama">Açıklama</label>
                                                                                <div class="col-sm-10">
                                                                                    <asp:TextBox ID="txtAciklama" runat="server" TextMode="MultiLine" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                                                                </div>
                                                                            </div>

                                                                            <div class="form-group">

                                                                                <label for="tarih2" class="col-md-2 control-label">İşlem Tarihi</label>
                                                                                <div class="col-md-10">

                                                                                    <input type='text' id="tarih2" runat="server" class="form-control" />

                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">
                                                                                <asp:Button ID="btnKaydet" runat="server" Text="Kaydet" CausesValidation="true" ValidationGroup="valGrup" CssClass="btn btn-info btn-block" OnClick="btnKaydet_Click" />
                                                                            </div>
                                                                        </div>

                                                                    </asp:Panel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="grdCihaz" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>

                                        <div class="modal-footer">
                                            <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Kapat</button>
                                        </div>
                                    </div>


                                </div>

                            </div>
                            <!-- Add Record Modal Starts here-->
                            <div id="addModalCihaz" class="modal  fade" tabindex="-1" role="dialog"
                                aria-labelledby="addModalCihazLabel" aria-hidden="true">
                                <div class="modal-dialog modal-content modal-sm">
                                    <div class="modal-header modal-header-info">
                                        <button type="button" class="close" data-dismiss="modal"
                                            aria-hidden="true">
                                            ×</button>
                                        <h3 id="addModalCihazLabel" class="baslik">Yeni Ürün/Parça</h3>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <div class="modal-body">

                                                <div class="form-horizontal">

                                                    <div class="form-group">
                                                        <asp:Label runat="server" AssociatedControlID="txtCihazAdi" CssClass="col-md-10 control-label">Malzeme Adı</asp:Label>
                                                        <div class="col-md-10">
                                                            <asp:TextBox runat="server" ID="txtCihazAdi" CssClass="form-control" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtCihazAdi" ErrorMessage="Lütfen Ürün/Parça adını giriniz"></asp:RequiredFieldValidator>
                                                            <asp:HiddenField ID="hdnGarantiSure" runat="server" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label runat="server" AssociatedControlID="txtCihazAciklama" CssClass="col-md-10 control-label">Açıklama</asp:Label>
                                                        <div class="col-md-10">
                                                            <asp:TextBox runat="server" ID="txtCihazAciklama" CssClass="form-control" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtCihazAciklama" ErrorMessage="Lütfen  açıklama giriniz"></asp:RequiredFieldValidator>

                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Label runat="server" AssociatedControlID="txtGarantiSuresi" CssClass="col-md-10 control-label">Garanti(AY)</asp:Label>
                                                        <div class="col-md-10">
                                                            <asp:TextBox runat="server" ID="txtGarantiSuresi" CssClass="form-control" TextMode="Number" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtGarantiSuresi" ErrorMessage="Lütfen  süre giriniz"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>


                                            <div class="modal-footer">
                                                <asp:Button ID="btnAddCihazRecord" runat="server" Text="Kaydet"
                                                    CssClass="btn btn-info" OnClick="btnAddCihazRecord_Click" ValidationGroup="musteriGrup2" />
                                                <button class="btn btn-info" data-dismiss="modal"
                                                    aria-hidden="true">
                                                    Kapat</button>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>

                                            <asp:AsyncPostBackTrigger ControlID="btnAddCihaz" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                            <%-- yeni makine hesabı seçimi başlıyor --%>
                            <div id="yeniMakineModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="yeniMakineModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-content modal-lg">
                                    <div class="modal-header modal-header-info">
                                        <a class="close" data-dismiss="modal" aria-hidden="true">×</a>
                                        <h3 id="yeniMakineModalLabel" class="baslik">Yeni Makine Çalışma Hesabı</h3>
                                    </div>
                                    <div class="modal-body">
                                        <div class="panel panel-info">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseYeniMakine" class="collapsed">Makine Seçimi</a>
                                                </h4>
                                            </div>
                                            <div id="collapseYeniMakine" class="panel-collapse collapse" style="height: 0px;">
                                                <div class="panel-body">
                                                    <!-- Müşteri seçim alanı başlıyor -->

                                                    <div class="table-responsive">
                                                        <asp:Panel runat="server" ID="panelMakine" DefaultButton="btnMakine">


                                                            <div class="input-group custom-search-form ">
                                                                <span class="input-group-btn">

                                                                    <asp:LinkButton ID="btnMakine"
                                                                        runat="server"
                                                                        CssClass="btn btn-info" OnClick="MakineAra"
                                                                        Text="<i class='fa fa-search icon-2x'></i>" />
                                                                </span>
                                                                <input runat="server" type="text" id="txtMakineAra" class="form-control" placeholder="Ara..." />

                                                            </div>
                                                        </asp:Panel>
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="grdMakine" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="makine_id"
                                                                    EmptyDataText="Ürün/Parça girilmemiş" OnSelectedIndexChanged="grdMakine_SelectedIndexChanged">
                                                                    <SelectedRowStyle CssClass="danger" />
                                                                    <Columns>
                                                                        <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Seç" HeaderText="Seçim">
                                                                            <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                                                                        </asp:ButtonField>
                                                                        <asp:BoundField DataField="makine_id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                            <HeaderStyle CssClass="visible-lg" />
                                                                            <ItemStyle CssClass="visible-lg" />
                                                                        </asp:BoundField>

                                                                        <asp:TemplateField HeaderText="Makine Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">

                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnRandom"
                                                                                    runat="server"
                                                                                    CssClass="btn btn-primary"
                                                                                    CommandName="detail" CommandArgument='<%#Eval("makine_id") %>' Text=' <%#Eval("adi") %> '>
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>

                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="plaka" HeaderText="Plaka"></asp:BoundField>
                                                                        <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                            <HeaderStyle CssClass="visible-lg" />
                                                                            <ItemStyle CssClass="visible-lg" />
                                                                        </asp:BoundField>

                                                                    </Columns>

                                                                </asp:GridView>
                                                                <asp:Button ID="btnAddMakine" runat="server" Text="Yeni Makine" CssClass="btn btn-info"
                                                                    OnClick="btnAddMakine_Click" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnMakine" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="grdMakine" EventName="RowCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnEkleHMakine" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnEkleHKMakine" EventName="Click" />

                                                            </Triggers>
                                                        </asp:UpdatePanel>


                                                    </div>

                                                    <!-- Müşteri seç,malanı bitiyor-->
                                                </div>
                                            </div>
                                        </div>

                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                            <ContentTemplate>

                                                <div class="panel panel-info">
                                                    <div class="panel-heading">
                                                        <h4 class="panel-title">
                                                            <a data-toggle="collapse" data-parent="#accordion" href="#collapseKararilMakine">Makine Çalışma Bilgileri</a>
                                                        </h4>
                                                    </div>
                                                    <div id="collapseKararilMakine" class="panel-collapse in" style="height: auto;">
                                                        <div class="panel-body">
                                                            <asp:Panel runat="server" ID="panelMakine2" DefaultButton="btnKaydetMakine">

                                                                <div class="form-horizontal">

                                                                    <div class="form-group">
                                                                        <label class="col-sm-2 control-label" for="drdTarife">Tarife</label>
                                                                        <div class="col-sm-4">
                                                                            <asp:DropDownList ID="drdTarife" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="drdTarife_SelectedIndexChanged">
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                        <label class="col-sm-2 control-label" for="txtFiyat">Birim Ücret</label>
                                                                        <div class="col-sm-4">
                                                                            <asp:TextBox runat="server" ID="txtFiyat" Enabled="false" Text="10" class="form-control" />
                                                                        </div>

                                                                    </div>

                                                                    <div runat="server" id="tarih_aralik">
                                                                        <div class="form-group">
                                                                            <label class="col-sm-2 control-label" for="datetimepicker6">Başlama</label>
                                                                            <div class="col-sm-4">
                                                                                <%--<input type='text' runat="server" class="form-control" id="datetimepicker6" />--%>
                                                                                <asp:TextBox runat="server" ID="datetimepicker6" class="form-control" AutoPostBack="true" />

                                                                            </div>
                                                                            <label class="col-sm-2 control-label" for="datetimepicker7">Bitiş</label>
                                                                            <div class="col-sm-4">
                                                                                <%--<input type='text' runat="server" class="form-control" id="datetimepicker7" />--%>
                                                                                <asp:TextBox runat="server" ID="datetimepicker7" class="form-control" AutoPostBack="true" />
                                                                            </div>

                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="col-sm-2 control-label" for="txtSure">Süre</label>
                                                                            <div class="input-group col-sm-6 ">
                                                                                <asp:TextBox runat="server" ID="txtSure" class="form-control" AutoPostBack="true" OnTextChanged="txtSure_TextChanged" />
                                                                                <span class="input-group-btn">
                                                                                    <asp:Button ID="btnSureHesapla" runat="server" Text="Hesapla" CssClass="btn btn-info" OnClick="btnSureHesapla_Click" />
                                                                                </span>

                                                                            </div>
                                                                        </div>

                                                                    </div>

                                                                    <div runat="server" id="numara_aralik">
                                                                        <div class="form-group">
                                                                            <label for="txtSonNumara" class="col-sm-2 control-label">Başlama</label>
                                                                            <div class="col-sm-4">
                                                                                <asp:TextBox runat="server" ID="txtSonNumara" AutoPostBack="true" OnTextChanged="txtBaslangicChanged" class="form-control" />
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" EnableClientScript="true" ControlToValidate="txtSonNumara" ErrorMessage="Lütfen baslangic saati" CssClass="text-danger" ValidationGroup="valGrupMakine"></asp:RequiredFieldValidator>
                                                                                <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="txtSonNumara" Type="Currency" ValidationGroup="valGrupMakine" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>
                                                                            </div>
                                                                            <label for="txtYeniNumara" class="col-sm-2 control-label">Bitiş</label>
                                                                            <div class="col-sm-4">
                                                                                <asp:TextBox runat="server" ID="txtYeniNumara" AutoPostBack="true" OnTextChanged="txtBaslangicChanged" class="form-control" />
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" EnableClientScript="true" ControlToValidate="txtYeniNumara" ErrorMessage="Lütfen baslangic saati" CssClass="text-danger" ValidationGroup="valGrupMakine"></asp:RequiredFieldValidator>
                                                                                <asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="txtYeniNumara" Type="Currency" ValidationGroup="valGrupMakine" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>
                                                                            </div>
                                                                            <asp:HiddenField ID="hdnSaatlik" runat="server" />
                                                                            <asp:HiddenField ID="hdnTarifeTipi" runat="server" />

                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label for="txtDakika" class="col-sm-2 control-label">Dakika</label>
                                                                            <div class="col-sm-4">

                                                                                <asp:TextBox runat="server" ID="txtDakika" AutoPostBack="true" class="form-control" OnTextChanged="txtSaat_TextChanged" />
                                                                            </div>
                                                                            <label for="txtSaatBilgi" class="col-sm-2 control-label">Saat</label>
                                                                            <div class="col-sm-4">

                                                                                <asp:TextBox runat="server" ID="txtSaatBilgi" Enabled="false" class="form-control" />
                                                                            </div>
                                                                        </div>
                                                                    </div>



                                                                    <div class="form-group">
                                                                        <label class="col-sm-2 control-label" for="txtMakineAdiGoster">Makine</label>
                                                                        <div class="col-sm-10">
                                                                            <input id="txtMakineAdiGoster" runat="server" type="text" class="form-control" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label class="col-sm-2 control-label" for="txtIslemParcaMakine">Yapılacak İşlem</label>
                                                                        <div class="col-sm-10">
                                                                            <input id="txtIslemParcaMakine" runat="server" type="text" class="form-control" />
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" EnableClientScript="true" CssClass="text-danger" ControlToValidate="txtIslemParcaMakine" ErrorMessage="Lütfen yapılacak işlemi giriniz" ValidationGroup="valGrupMakine"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>

                                                                    <div class="form-group">

                                                                        <label class="col-sm-2 control-label" for="txtKDVOraniDuzenleMakine">KDV Oranı</label>
                                                                        <div class="col-sm-10">
                                                                            <asp:TextBox ID="txtKDVOraniDuzenleMakine" runat="server" Text="18" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div hidden class="form-group">

                                                                        <label class="col-sm-2 control-label" for="txtKDVDuzenleMakine">KDV Tutarı</label>
                                                                        <div class="col-sm-10">
                                                                            <asp:TextBox ID="txtKDVDuzenleMakine" Enabled="false" runat="server" Text="18" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">

                                                                        <label class="col-sm-2 control-label" for="txtYekunMakine">Tutar(KDV Dahil)</label>
                                                                        <div class="col-sm-10">
                                                                            <asp:TextBox ID="txtYekunMakine" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" EnableClientScript="true" ControlToValidate="txtYekunMakine" ErrorMessage="Lütfen toplam tutar giriniz" CssClass="text-danger" ValidationGroup="valGrupMakine"></asp:RequiredFieldValidator>
                                                                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtYekunMakine" Type="Currency" ValidationGroup="valGrupMakine" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>
                                                                            <asp:HiddenField ID="hdnHesapIDDuzenMakine" runat="server" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">

                                                                        <label class="col-sm-2 control-label" for="txtAciklamaMakine">Açıklama</label>
                                                                        <div class="col-sm-10">
                                                                            <asp:TextBox ID="txtAciklamaMakine" runat="server" TextMode="MultiLine" CssClass="form-control" ValidationGroup="valGrupMakine"></asp:TextBox>
                                                                        </div>
                                                                    </div>

                                                                    <div class="form-group">

                                                                        <label for="txtTarihMakine" class="col-md-2 control-label">İşlem Tarihi</label>
                                                                        <div class="col-md-10">

                                                                            <input type='text' id="txtTarihMakine" runat="server" class="form-control" />

                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Button ID="btnKaydetMakine" runat="server" Text="Kaydet" CausesValidation="true" ValidationGroup="valGrupMakine" CssClass="btn btn-info btn-block" OnClick="btnKaydetMakine_Click" />
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="grdMakine" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <div class="modal-footer">
                                            <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Kapat</button>
                                        </div>
                                    </div>

                                </div>

                            </div>
                            <%-- makine hesabı seçimi bitiyor --%>
                        </div>

                        <div class=" btn-group pull-right visible-lg">

                            <asp:LinkButton ID="btnEkleH"
                                runat="server"
                                CssClass="btn btn-info" OnClick="btnEkle_ClickH"
                                Text="<i class='fa fa-plus icon-2x'></i>" />
                            <asp:LinkButton ID="btnEkleHMakine"
                                runat="server"
                                CssClass="btn btn-danger" OnClick="btnEkleMakine_ClickH"
                                Text="<i class='fa fa-plus icon-2x'></i>" />

                            <asp:Button ID="btnTamirciye" runat="server" Text="Dış Hizmet" CssClass="btn btn-info"
                                OnClick="btnTamirciye_Click" />

                            <asp:LinkButton ID="btnTopluOnay"
                                runat="server"
                                CssClass="btn btn-info" OnClick="btnTopluOnay_Click"
                                Text="<i class='fa fa-check icon-2x'></i>" />
                            <asp:LinkButton ID="btnPrint"
                                runat="server"
                                CssClass="btn btn-info visible-lg" OnClick="btnPrnt_Click"
                                Text="<i class='fa fa-print icon-2x'></i>" />

                            <asp:LinkButton ID="btnExportExcel"
                                runat="server"
                                CssClass="btn btn-info visible-lg " OnClick="btnExportExcel_Click"
                                Text="<i class='fa fa-file-excel-o icon-2x'></i>" />

                            <asp:LinkButton ID="btnExportWord"
                                runat="server"
                                CssClass="btn btn-info visible-lg" OnClick="btnExportWord_Click"
                                Text="<i class='fa fa-wikipedia-w icon-2x'></i>" />


                        </div>

                        <div class=" btn-group pull-right visible-xs visible-sm">


                            <asp:LinkButton ID="btnEkleHK"
                                runat="server"
                                CssClass="btn btn-info" OnClick="btnEkle_ClickH"
                                Text="<i class='fa fa-plus icon-2x'></i>" />
                            <asp:LinkButton ID="btnEkleHKMakine"
                                runat="server"
                                CssClass="btn btn-danger" OnClick="btnEkleMakine_ClickH"
                                Text="<i class='fa fa-plus icon-2x'></i>" />


                            <asp:LinkButton ID="btnTamirciyeK"
                                runat="server"
                                CssClass="btn btn-info" OnClick="btnTamirciye_Click"
                                Text="<i class='fa fa-send icon-2x'></i>" />


                            <asp:LinkButton ID="btnTopluOnayK"
                                runat="server"
                                CssClass="btn btn-info" OnClick="btnTopluOnay_Click"
                                Text="<i class='fa fa-thumbs-up icon-2x'></i>" />



                        </div>
                    </div>

                    <%--   <div class="panel-footer pull-right">
                    </div>--%>
                </div>

            </div>
            <div id="Div4" runat="server" class="panel panel-info">
                <div class="panel-heading">
                    <h2 class="panel-title">
                        <a data-toggle="collapse" data-parent="#accordion" href="#collapseTeorik" class="collapsed">Tanımlı Sarf Tüketim Hesapları</a>

                    </h2>

                </div>

                <div id="collapseTeorik" class="panel-collapse collapse" style="height: 0px;">
                    <div class="table-responsive">
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                            <ContentTemplate>
                                <div id="Div6" runat="server" class="pull-right">
                                    <span id="txtAdetT" runat="server" class="label label-success"></span>
                                    <span id="txtMiktarT" runat="server" class="label label-success"></span>
                                    <span id="txtTutarT" runat="server" class="label label-warning"></span>
                                </div>
                                <asp:GridView ID="grdAlimlarTeorik" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="id"
                                    EmptyDataText="teorik masraf kaydı yok" AllowPaging="true" PageSize="20">

                                    <PagerStyle CssClass="pagination-ys" />
                                    <Columns>
                                        <asp:BoundField DataField="makine_plaka" HeaderText="Makine Plaka"></asp:BoundField>
                                        <asp:BoundField DataField="masraf" HeaderText="Masraf"></asp:BoundField>
                                        <asp:BoundField DataField="miktar" HeaderText="Miktar" />
                                        <asp:BoundField DataField="tutar" HeaderText="Tutar" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                        <asp:BoundField DataField="tarih" HeaderText="Tarih" />
                                    </Columns>

                                </asp:GridView>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div id="Div5" runat="server" class="panel panel-info">
                <div class="panel-heading">
                    <h2 class="panel-title">
                        <a data-toggle="collapse" data-parent="#accordion" href="#collapseGiris" class="collapsed">Tarih Aralığındaki Sarf Tüketimleri</a>
                    </h2>

                </div>
                <div id="collapseGiris" class="panel-collapse collapse" style="height: 0px;">
                    <div class="table-responsive">
                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                            <ContentTemplate>
                                <div id="cariOzetG" runat="server" class="pull-right">
                                    <span id="txtAdetG" runat="server" class="label label-success"></span>
                                    <span id="txtMiktarG" runat="server" class="label label-success"></span>
                                    <span id="txtTutarG" runat="server" class="label label-warning"></span>
                                </div>

                                <asp:GridView ID="grdAlimlarGirisler" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="id"
                                    EmptyDataText="Masraf girişi bulunmuyor" OnRowCommand="grdAlimlarGirisler_RowCommand"
                                    AllowPaging="true" PageSize="10">

                                    <PagerStyle CssClass="pagination-ys" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDel"
                                                    runat="server"
                                                    CssClass="btn btn-danger"
                                                    CommandName="del" CommandArgument='<%# Eval("id") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("makine_adi") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>

                                                <asp:LinkButton ID="btnTedarikci"
                                                    runat="server"
                                                    CssClass="btn btn-primary"
                                                    CommandName="detail" CommandArgument='<%#Eval("makine_id") %>' Text=' <%#Eval("makine_adi") %> '>
                           
                                                </asp:LinkButton>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:BoundField DataField="makine_plaka" HeaderText="Plaka"></asp:BoundField>
                                        <asp:BoundField DataField="masraf_id" HeaderText="MasrafID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                        <asp:BoundField DataField="masraf_adi" HeaderText="Masraf"></asp:BoundField>
                                        <asp:BoundField DataField="miktar" HeaderText="Miktar" />
                                        <asp:BoundField DataField="tutar" HeaderText="Tutar" DataFormatString="{0:C}" />
                                        <asp:BoundField DataField="aciklama" HeaderText="Açıklama" />
                                        <asp:BoundField DataField="tarih" HeaderText="Tarih" />

                                    </Columns>

                                </asp:GridView>

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="grdAlimlarGirisler" EventName="RowCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>

            <div id="Div7" runat="server" class="panel panel-info">
                <div class="panel-heading">
                    <h2 class="panel-title">
                        <a data-toggle="collapse" data-parent="#accordion" href="#collapseAtama" class="collapsed">Şantiyeye Atanmış Operatörler</a>
                    </h2>

                </div>
                <div id="collapseAtama" class="panel-collapse collapse" style="height: 0px;">
                    <div class="table-responsive">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grdAtamalar" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="id"
                                    EmptyDataText="Operatör ataması bulunmuyor" OnRowCommand="grdAtamalar_RowCommand"
                                    AllowPaging="true" PageSize="10">

                                    <PagerStyle CssClass="pagination-ys" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDel"
                                                    runat="server"
                                                    CssClass="btn btn-danger"
                                                    CommandName="del" CommandArgument='<%# Eval("id") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'>Sil</i>" />
                                                <asp:LinkButton ID="btnCikarrr"
                                                    runat="server"
                                                    CssClass="btn btn-success"
                                                    CommandName="cikis" CommandArgument='<%# Eval("id") %>' Text="<i class='fa fa-check'>Çıkar</i>" />

                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:BoundField DataField="id" HeaderText="ID"></asp:BoundField>
                                        <asp:BoundField DataField="kullanici" HeaderText="Operatör"></asp:BoundField>

                                        <asp:BoundField DataField="atama" HeaderText="Atanma" />
                                        <asp:BoundField DataField="cikarma" HeaderText="Çıkış" />

                                    </Columns>

                                </asp:GridView>

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="grdAtamalar" EventName="RowCommand" />
                                <asp:AsyncPostBackTrigger ControlID="btnAta" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <div class=" btn-group pull-right visible-lg">

                            <asp:LinkButton ID="btnAta"
                                runat="server"
                                CssClass="btn btn-info" OnClick="btnAta_Click"
                                Text="<i class='fa fa-plus icon-2x'>Yeni</i>" />

                        </div>
                        <div id="atamaModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="atamaModalLabel" aria-hidden="true">
                            <div class="modal-dialog modal-content modal-lg">
                                <%--     <div class="modal-header modal-header-info">
                                    <a class="close" data-dismiss="modal" aria-hidden="true">×</a>
                                    <h3 id="atamaModalLabel" class="baslik">Operatör Atama</h3>
                                </div>--%>

                                <div class="modal-body">

                                    <div class="panel panel-info">

                                        <div class="panel-body">
                                            <div class="panel panel-info">
                                                <div class="panel-heading">
                                                    <h4 class="panel-title">Operatör Seçimi
                                                    </h4>
                                                </div>

                                                <div class="panel-body">
                                                    <!-- Müşteri seçim alanı başlıyor -->

                                                    <div class="table-responsive">
                                                        <asp:Panel runat="server" ID="panel1" DefaultButton="btnOperatorAra">

                                                            <div class="input-group custom-search-form ">
                                                                <span class="input-group-btn">
                                                                    <asp:LinkButton ID="btnOperatorAra"
                                                                        runat="server"
                                                                        CssClass="btn btn-info" OnClick="OperatorAra"
                                                                        Text="<i class='fa fa-search icon-2x'></i>" />
                                                                </span>
                                                                <input runat="server" type="text" id="txtOperatorAra" class="form-control" placeholder="Ara..." />

                                                            </div>
                                                        </asp:Panel>
                                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                            <ContentTemplate>

                                                                <asp:GridView ID="grdOperator" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="userName"
                                                                    EmptyDataText="Hiçbir operatör tanımlanmamış" OnSelectedIndexChanged="grdOperator_SelectedIndexChanged">
                                                                    <SelectedRowStyle CssClass="danger" />
                                                                    <Columns>

                                                                        <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Seç" HeaderText="Seçim">
                                                                            <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                                                                        </asp:ButtonField>


                                                                        <asp:BoundField DataField="userName" HeaderText="Operatör" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                            <HeaderStyle CssClass="visible-lg" />
                                                                            <ItemStyle CssClass="visible-lg" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="tel" HeaderText="Telefon" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                            <HeaderStyle CssClass="visible-lg" />
                                                                            <ItemStyle CssClass="visible-lg" />
                                                                        </asp:BoundField>
                                                                    </Columns>

                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnOperatorAra" EventName="Click" />

                                                            </Triggers>
                                                        </asp:UpdatePanel>


                                                    </div>

                                                    <!-- Müşteri seç,malanı bitiyor-->
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Kapat</button>
                                </div>

                            </div>

                        </div>
                        <div id="cikisModal" class="modal  fade" tabindex="-1" role="dialog"
                            aria-labelledby="cikisModalLabel" aria-hidden="true">
                            <div class="modal-dialog modal-content modal-sm">

                                <asp:UpdatePanel ID="UpdatePanel13" runat="server">

                                    <ContentTemplate>
                                        <div class="modal-body">
                                            <div class="row">

                                                <div class="col-md-12">
                                                    <div class="alert alert-info text-center">
                                                        <i class="fa fa-2x">Operatör Şantiye Çıkışını Onaylıyor musunuz?</i>

                                                        <div class="btn-group pull-right">
                                                            <asp:Button ID="btnCikarKaydet" runat="server" Text="Tamam"
                                                                CssClass="btn btn-success" OnClick="btnCikarKaydet_Click" />
                                                            <button class="btn btn-warning" data-dismiss="modal"
                                                                aria-hidden="true">
                                                                Kapat</button>
                                                        </div>
                                                    </div>
                                                </div>

                                                <asp:HiddenField ID="hdnServisOpID" runat="server" />

                                            </div>
                                        </div>

                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnCikarKaydet" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <div id="onayModal" class="modal  fade" tabindex="-1" role="dialog"
        aria-labelledby="addModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-content modal-sm">

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">

                <ContentTemplate>
                    <div class="modal-body">
                        <div class="row">

                            <div class="col-md-12">
                                <div class="alert alert-info text-center">
                                    <i class="fa fa-2x">İşi sonlandırmak istiyor musunuz?</i>

                                    <div class="btn-group pull-right">

                                        <asp:Button ID="btnOnay" runat="server" Text="Tamam"
                                            CssClass="btn btn-success" OnClick="btnOnay_Click" />
                                        <button class="btn btn-warning" data-dismiss="modal"
                                            aria-hidden="true">
                                            Kapat</button>

                                    </div>
                                </div>
                            </div>

                            <asp:HiddenField ID="hdnHesapID" runat="server" />
                            <asp:HiddenField ID="hdnMusteriID" runat="server" />
                            <asp:HiddenField ID="hdnServisIDD" runat="server" />

                        </div>
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSonlandir" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSonlandirK" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="pager" runat="server">
        <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="5">

            <Fields>
                <asp:NextPreviousPagerField ShowLastPageButton="False" ShowNextPageButton="False" ButtonType="Button" ButtonCssClass="btn btn-danger" RenderNonBreakingSpacesBetweenControls="false" />
                <asp:NumericPagerField ButtonType="Button" RenderNonBreakingSpacesBetweenControls="false" NumericButtonCssClass="btn btn-primary" CurrentPageLabelCssClass="btn btn-primary disabled" NextPreviousButtonCssClass="btn" />
                <asp:NextPreviousPagerField ShowFirstPageButton="False" ShowPreviousPageButton="False" ButtonType="Button" ButtonCssClass="btn btn-danger" RenderNonBreakingSpacesBetweenControls="false" />
            </Fields>
        </asp:DataPager>

    </div>
    <asp:ListView ID="ListView1" runat="server" OnItemDataBound="ListView1_ItemDataBound" OnDataBound="ListView1_DataBound" OnPagePropertiesChanging="ListView1_PagePropertiesChanging">
        <ItemTemplate>

            <div id="Div1" runat="server" class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title"><%#DataBinder.Eval(Container.DataItem,"tarihZaman")%> - <%#DataBinder.Eval(Container.DataItem,"kullanici")%> 
                    </h4>
                </div>
                <div class="panel-body">


                    <h3><%#DataBinder.Eval(Container.DataItem,"baslik")%></h3>


                    <p class="lead"><%#DataBinder.Eval(Container.DataItem,"aciklama")%> </p>


                    <div runat="server" id="resimCerceve">

                        <img id="resHTML" class="img-responsive img-rounded" runat="server" src='<%# Eval("belgeYol") %>' />

                        <asp:TextBox ID="txtYol" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"belgeYol")%>'></asp:TextBox>

                    </div>
                </div>
            </div>

        </ItemTemplate>

    </asp:ListView>


    <div id="Div3" runat="server">
        <asp:DataPager ID="DataPager2" runat="server" PagedControlID="ListView1" PageSize="5">

            <Fields>
                <asp:NextPreviousPagerField ShowLastPageButton="False" ShowNextPageButton="False" ButtonType="Button" ButtonCssClass="btn btn-danger" RenderNonBreakingSpacesBetweenControls="false" />
                <asp:NumericPagerField ButtonType="Button" RenderNonBreakingSpacesBetweenControls="false" NumericButtonCssClass="btn btn-primary" CurrentPageLabelCssClass="btn btn-primary disabled" NextPreviousButtonCssClass="btn" />
                <asp:NextPreviousPagerField ShowFirstPageButton="False" ShowPreviousPageButton="False" ButtonType="Button" ButtonCssClass="btn btn-danger" RenderNonBreakingSpacesBetweenControls="false" />
            </Fields>
        </asp:DataPager>

    </div>



    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('#ContentPlaceHolder1_tarih2').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
            $('#ContentPlaceHolder1_txtTarihMakine').datetimepicker({
                format: 'L',

                locale: 'tr'
            });

            $('#ContentPlaceHolder1_tarihtamirci').datetimepicker({
                format: 'L',

                locale: 'tr'
            });

            $('#ContentPlaceHolder1_datetimepicker6').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
            $('#ContentPlaceHolder1_datetimepicker7').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
            $("#ContentPlaceHolder1_datetimepicker6").on("dp.change", function (e) {
                $('#ContentPlaceHolder1_datetimepicker7').data("DateTimePicker").minDate(e.date);
            });
            $("#ContentPlaceHolder1_datetimepicker7").on("dp.change", function (e) {
                $('#ContentPlaceHolder1_datetimepicker6').data("DateTimePicker").maxDate(e.date);
            });
        }

    </script>
</asp:Content>
