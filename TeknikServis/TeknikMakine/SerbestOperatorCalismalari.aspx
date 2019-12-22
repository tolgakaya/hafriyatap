<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="SerbestOperatorCalismalari.aspx.cs" Inherits="TeknikServis.TeknikMakine.SerbestOperatorCalismalari" %>

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

        <div id="Div2" runat="server" class="panel panel-info">
            <div class="panel-heading">
                <h2 class="panel-title">Onaylanmamış Operatör Çalışmaları
                </h2>
            </div>

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

                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                DataKeyNames="hesapID"
                                EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand"
                                OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" AllowPaging="true" PageSize="10">
                                <PagerStyle CssClass="pagination-ys" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" HeaderStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="delLink"
                                                runat="server"
                                                CssClass="btn btn-danger"
                                                CommandName="del" CommandArgument='<%#Eval("hesapID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'>Sil</i>" />
                                            <asp:LinkButton ID="btnCikarrr"
                                                runat="server"
                                                CssClass="btn btn-success"
                                                CommandName="cikis" CommandArgument='<%# Eval("hesapID") %>' Text="<i class='fa fa-check'>Onayla</i>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm" ItemStyle-CssClass="visible-xs visible-sm">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="delLinkk"
                                                runat="server"
                                                CssClass="btn btn-danger btn-md"
                                                CommandName="del" CommandArgument='<%#Eval("hesapID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
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
                                    <asp:BoundField DataField="cihaz" HeaderText="Makine" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                    <asp:BoundField DataField="dakika" HeaderText="Dakika" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                    <asp:BoundField DataField="tarife" HeaderText="Tarife"></asp:BoundField>
                                    <asp:BoundField DataField="sure" HeaderText="Süre"></asp:BoundField>
                                    <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                     <asp:BoundField DataField="kullanici" HeaderText="Operatör"></asp:BoundField>
                                </Columns>

                            </asp:GridView>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />

                        </Triggers>
                    </asp:UpdatePanel>

                    <%-- servishesapları bitiyor --%>

                    <%-- yeni makine hesabı seçimi başlıyor --%>
                    <div id="yeniMakineModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="yeniMakineModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-content modal-lg">


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
                                                        <asp:HiddenField ID="hdnMakineID" runat="server" />
                                                        <asp:HiddenField ID="hdnHesapID" runat="server" />
                                                        <div class="form-group">
                                                            <label class="col-sm-2 control-label" for="txtMakine">Makine</label>
                                                            <div class="col-sm-10">
                                                                <input id="txtMakine" runat="server" type="text" disabled class="form-control" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-sm-2 control-label" for="drdTarife">Tarife</label>
                                                            <div class="col-sm-4">
                                                                <asp:DropDownList ID="drdTarife" runat="server" AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>

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
                                                                    <asp:TextBox runat="server" ID="txtSure" class="form-control" />
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <asp:HiddenField ID="hdnSaatlik" runat="server" />
                                                        <asp:HiddenField ID="hdnTarifeTipi" runat="server" />

                                                        <div runat="server" id="numara_aralik">
                                                            <div class="form-group">
                                                                <label for="txtSonNumara" class="col-sm-2 control-label">Başlama</label>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox runat="server" ID="txtSonNumara" AutoPostBack="true" class="form-control" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" EnableClientScript="true" ControlToValidate="txtSonNumara" ErrorMessage="Lütfen baslangic saati" CssClass="text-danger" ValidationGroup="valGrupMakine"></asp:RequiredFieldValidator>
                                                                    <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="txtSonNumara" Type="Currency" ValidationGroup="valGrupMakine" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>
                                                                </div>
                                                                <label for="txtYeniNumara" class="col-sm-2 control-label">Bitiş</label>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox runat="server" ID="txtYeniNumara" AutoPostBack="true" class="form-control" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" EnableClientScript="true" ControlToValidate="txtYeniNumara" ErrorMessage="Lütfen baslangic saati" CssClass="text-danger" ValidationGroup="valGrupMakine"></asp:RequiredFieldValidator>
                                                                    <asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="txtYeniNumara" Type="Currency" ValidationGroup="valGrupMakine" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>
                                                                </div>

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
                                                            <label class="col-sm-2 control-label" for="txtIslemParcaMakine">Şantiye Adı/Açıklama</label>
                                                            <div class="col-sm-10">
                                                                <input id="txtAciklama" runat="server" type="text" class="form-control" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">

                                                            <label for="txtTarihMakine" class="col-md-2 control-label">İşlem Tarihi</label>
                                                            <div class="col-md-10">

                                                                <input type='text' id="txtTarihMakine" runat="server" class="form-control" />

                                                            </div>
                                                        </div>
                                                                 <div class="form-group">
                                                                     <asp:HiddenField ID="hdnCari" runat="server" />
                                            <label class="col-md-2 control-label" for="txtCariID">Kişi/Firma Seç</label>
                                            <div class="col-md-10">
                                    <div class="input-group custom-search-form ">
                                    <span class="input-group-btn">
                                        <button id="Button1" runat="server" class="btn btn-default" type="submit" onserverclick="Button1_ServerClick">
                                            <i class="fa fa-search">Seç</i>
                                        </button>
                                    </span>
                                    <input runat="server" type="text" id="txtCariID" class="form-control" placeholder="Cari seç..." />
                                    </div
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

                                    <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                                </Triggers>
                            </asp:UpdatePanel>

                            <div class="modal-footer">
                                <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Kapat</button>
                            </div>
                        </div>

                        <%-- buraaaa --%>
                        <%-- </div>--%>
                    </div>
                    <%-- makine hesabı seçimi bitiyor --%>
                </div>

            </div>

            <%--   <div class="panel-footer pull-right">
                    </div>--%>
        </div>
            <div id="musteriModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="tamirciModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-lg">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="tamirciModalLabel" class="baslik">Müşteri Seçimi</h3>
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
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseMusteri" class="collapsed">Müşteri Seçimi</a>
                            </h4>
                        </div>


                        <%-- müşteri aramayı ekleyecez --%>
                        <div id="collapseMusteri" class="panel-collapse in" style="height: auto;">
                            <div class="panel-body">
                                <!-- Müşteri seçim alanı başlıyor -->

                                <div class="table-responsive">
                                    <asp:Panel runat="server" ID="musteriPanel" DefaultButton="btnMusteriAra">
                                        <div class="input-group custom-search-form">

                                            <input runat="server" type="text" id="txtMusteriSorgu" class="form-control" placeholder="Ara..." />
                                            <span class="input-group-btn">
                                                <asp:Button Text="Ara" runat="server" ID="btnMusteriAra" class="btn btn-default" OnClick="MusteriAra" />
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
                                OnSelectedIndexChanged="grdMusteri_SelectedIndexChanged">
                                <SelectedRowStyle CssClass="danger" />

                                <Columns>
                                    <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Seç">
                                        <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="CustID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Müşteri Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
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
