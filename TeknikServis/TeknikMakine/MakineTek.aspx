<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="MakineTek.aspx.cs" Inherits="TeknikServis.TeknikMakine.MakineTek" %>

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
        <div id="Div1" runat="server" class="panel panel-info">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseHop" class="collapsed">Arama Kriterleri</a>
                </h4>
            </div>
            <div id="collapseHop" class="panel-collapse collapse" style="height: 0px;">
                <div class="panel-body">
                    <div class="row ">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="datetimepicker6">Şu Tarihten:</label>
                                <input type='text' runat="server" class="form-control" id="datetimepicker6" />
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="datetimepicker7">Şu Tarihe:</label>
                                <input type='text' runat="server" class="form-control" id="datetimepicker7" />
                            </div>
                        </div>
                    </div>


                    <div class="form-group">
                        <button id="btnAra" runat="server" class="btn btn-info btn-lg btn-block" type="submit" onserverclick="Ara">Ara...</button>
                    </div>
                    <!--body-->
                </div>
            </div>
        </div>

        <div class="panel-group" id="accordionik">
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upCrudGrid">
                <ProgressTemplate>
                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                        <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="upCrudGrid" runat="server" ChildrenAsTriggers="true">

                <ContentTemplate>
                    <div id="panelContents" runat="server" class="panel panel-info">
                        <div class="panel-heading">
                            <h2 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" class="collapsed">Makine Bilgileri</a>
                            </h2>

                        </div>
                        <div id="collapseOne" class="panel-collapse in" style="height: auto;">
                            <div class="panel-body">

                                <div class="btn-group pull-right">

                                    <asp:Button ID="btnRapor" runat="server" CssClass="btn btn-info" Text="Rapor" OnClick="btnBelge_Click" />
                                    <asp:Button ID="btnMasrafGir" runat="server" CssClass="btn btn-info" Text="Sarf Kaydı" OnClick="btnMasrafGir_Click" />

                                </div>


                                <div class="col-md-12">
                                    <h3>
                                        <label id="txtMakine_plaka" runat="server" class="label label-danger"></label>
                                    </h3>
                                    <h3 id="txtSonSayac" runat="server"></h3>

                                    <p id="txtServisAciklama" runat="server" class="lead"></p>

                                    <p id="txtServisAdresi" runat="server" class="lead"></p>

                                </div>

                                <div class="col-md-6">

                                    <label for="txtToplamCalismaSaat">Toplam Çalışma /Saat:</label>
                                    <input class="form-control alert-info" id="txtToplamCalismaSaat" runat="server" type="text" disabled="disabled" />

                                    <label for="txtToplamCalismaGun">Toplam Çalışma /Gün:</label>
                                    <input class="form-control alert-warning" id="txtToplamCalismaGun" runat="server" />
                                    <label for="txtToplamCalismaHafta">Toplam Çalışma/Hafta:</label>
                                    <input class="form-control alert-warning" id="txtToplamCalismaHafta" runat="server" />
                                    <label for="txtToplamCalismaAy">Toplam Çalışma /Ay:</label>
                                    <input class="form-control alert-warning" id="txtToplamCalismaAy" runat="server" />

                                </div>
                                <div class="col-md-6">
                                    <label for="txtToplamMasrafTeorik">Tanımlanan Top Masraf:</label>
                                    <input class="form-control alert-info" id="txtToplamMasrafTeorik" runat="server" type="text" disabled="disabled" />

                                    <label for="txtToplamMasrafGercek">Gerçekleşen Toplam Masraf:</label>
                                    <input class="form-control alert-warning" id="txtToplamMasrafGercek" runat="server" />

                                    <label for="txtToplamGelir">Toplam Gelir:</label>
                                    <input class="form-control alert-info" id="txtToplamGelir" runat="server" type="text" disabled="disabled" />

                                    <label for="txtServisSayaci">Genel Servis Sayacı:</label>
                                    <input class="form-control alert-warning" id="txtServisSayaci" runat="server" />

                                </div>


                            </div>
                        </div>

                    </div>

                    <div id="Div2" runat="server" class="panel panel-info">
                        <div class="panel-heading">
                            <h2 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" class="collapsed">İş/Şantiye Hesapları</a>
                            </h2>
                        </div>
                        <div id="collapseTwo" class="panel-collapse collapse" style="height: 0px;">
                            <div class="panel-body">
                                <div class="table-responsive">
                                    <%-- servis hesapları başlıyor --%>

                                    <div id="cariOzet" runat="server" class="pull-right">
                                        <span id="txtHesapAdet" runat="server" class="label label-success"></span>
                                        <span id="txtHesapMaliyet" runat="server" class="label label-warning"></span>
                                        <span id="txtHesapTutar" runat="server" class="label label-info"></span>
                                    </div>
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                        DataKeyNames="hesapID"
                                        EmptyDataText="Kayıt girilmemiş" AllowPaging="true" PageSize="5" OnPageIndexChanging="GridView1_PageIndexChanging">
                                        <PagerStyle CssClass="pagination-ys" />
                                        <Columns>
                                            <asp:BoundField DataField="hesapID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                <HeaderStyle CssClass="visible-lg" />
                                                <ItemStyle CssClass="visible-lg" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>

                                            <asp:BoundField DataField="islemParca" HeaderText="İşlem" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="baslangic_tarih" HeaderText="Baş. Tarih" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                            <asp:BoundField DataField="bitis_tarih" HeaderText="Bit. Tarih" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>

                                            <asp:BoundField DataField="baslangic" HeaderText="Başlama" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                            <asp:BoundField DataField="bitis" HeaderText="Bitiş" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                            <asp:BoundField DataField="dakika" HeaderText="Dakika" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                            <asp:BoundField DataField="calisma_saati" HeaderText="Süre"></asp:BoundField>
                                            <asp:BoundField DataField="tarifekodu" HeaderText="Tarife"></asp:BoundField>
                                            <asp:BoundField DataField="toplam_maliyet" HeaderText="Maliyet"></asp:BoundField>
                                            <asp:BoundField DataField="yekun" HeaderText="Gelir" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                            <asp:BoundField DataField="tarihZaman" HeaderText="Onay Tarihi">
                                                <HeaderStyle CssClass="visible-lg" />
                                                <ItemStyle CssClass="visible-lg" />
                                            </asp:BoundField>

                                        </Columns>

                                    </asp:GridView>



                                    <%-- servishesapları bitiyor --%>
                                </div>

                            </div>

                            <%--   <div class="panel-footer pull-right">
                    </div>--%>
                        </div>

                    </div>
                    <div id="Div4" runat="server" class="panel panel-info">
                        <div class="panel-heading">
                            <h2 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseTeorik" class="collapsed">Sarf Tüketim Kayıtları(Ön Tanımlı)</a>
                            </h2>
                        </div>

                        <div id="collapseTeorik" class="panel-collapse collapse" style="height: 0px;">
                            <div class="table-responsive">

                                <div id="Div6" runat="server" class="pull-right">
                                    <span id="txtAdetT" runat="server" class="label label-success"></span>
                                    <span id="txtMiktarT" runat="server" class="label label-success"></span>
                                    <span id="txtTutarT" runat="server" class="label label-warning"></span>
                                </div>
                                <asp:GridView ID="grdAlimlarTeorik" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="id"
                                    EmptyDataText="teorik masraf kaydı yok" AllowPaging="true" PageSize="10" OnPageIndexChanging="grdAlimlarTeorik_PageIndexChanging">

                                    <PagerStyle CssClass="pagination-ys" />
                                    <Columns>

                                        <asp:BoundField DataField="makine_plaka" HeaderText="Makine Plaka"></asp:BoundField>
                                        <asp:BoundField DataField="masraf" HeaderText="Masraf"></asp:BoundField>
                                        <asp:BoundField DataField="miktar" HeaderText="Miktar" />
                                        <asp:BoundField DataField="birim" HeaderText="Birim" />
                                        <asp:BoundField DataField="tutar" HeaderText="Tutar" />
                                        <asp:BoundField DataField="tarih" HeaderText="Tarih" />

                                    </Columns>

                                </asp:GridView>


                            </div>
                        </div>
                    </div>
                    <div id="Div5" runat="server" class="panel panel-info">
                        <div class="panel-heading">
                            <h2 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseGiris" class="collapsed">Sarf Türketim Kayıtları</a>
                            </h2>

                        </div>
                        <div id="collapseGiris" class="panel-collapse collapse" style="height: 0px;">
                            <div class="table-responsive">

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
                                        <asp:BoundField DataField="birim" HeaderText="Birim" />
                                        <asp:BoundField DataField="tutar" HeaderText="Tutar" DataFormatString="{0:C}" />
                                        <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                        <asp:BoundField DataField="tarih" HeaderText="Tarih" />

                                    </Columns>

                                </asp:GridView>


                            </div>
                        </div>

                    </div>

                    <div id="Div3" runat="server" class="panel panel-info">
                        <div class="panel-heading">
                            <h2 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseSayac2" class="collapsed">Servis ve Bakım Sayaçları</a>
                            </h2>
                        </div>
                        <div id="collapseSayac2" class="panel-collapse collapse" style="height: 0px;">
                            <div class="table-responsive">

                                <asp:GridView ID="grdSayac" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="sayac_id"
                                    EmptyDataText="Servis sayaç tanımı bulunmuyor" AllowPaging="true" PageSize="3" OnPageIndexChanging="grdSayac_PageIndexChanging" OnRowDataBound="grdSayac_RowDataBound" OnRowCommand="grdSayac_RowCommand">

                                    <PagerStyle CssClass="pagination-ys" />
                                    <Columns>

                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <%--
                                                <asp:LinkButton ID="btnDetay"
                                                    runat="server"
                                                    CssClass="btn btn-success"
                                                    CommandName="duzenle" CommandArgument='<%#EvaBl("sayac_id")+ ";" + Container.DisplayIndex  %>' Text="<i class='fa fa-pencil'></i>" />
                                                --%>
                                                <asp:LinkButton ID="btnDel"
                                                    runat="server"
                                                    CssClass="btn btn-danger"
                                                    CommandName="del" CommandArgument='<%# Eval("sayac_id") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                                </div>
                                            </ItemTemplate>


                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Masraf">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("masraf_adi") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>

                                                <asp:LinkButton ID="btnTedarikci"
                                                    runat="server"
                                                    CssClass="btn btn-primary"
                                                    CommandName="detail" CommandArgument='<%#Eval("masraf_id") %>' Text=' <%#Eval("masraf_adi") %> '>
                           
                                                </asp:LinkButton>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:BoundField DataField="sayac" HeaderText="Sayaç"></asp:BoundField>

                                    </Columns>

                                </asp:GridView>
                                <asp:Button ID="btnyeniSayac" CssClass="btn btn-primary" Text="Yeni Sayaç" runat="server" OnClick="btnyeniSayac_Click" />

                            </div>
                        </div>

                    </div>
                    <div id="Div7" runat="server" class="panel panel-info">
                        <div class="panel-heading">
                            <h2 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseTanim" class="collapsed">Saatlik Sarf Tüketim Tanımları</a>
                            </h2>

                        </div>
                        <div id="collapseTanim" class="panel-collapse collapse" style="height: 0px;">
                            <div class="table-responsive">

                                <asp:GridView ID="grdTanim" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="tanim_id"
                                    EmptyDataText="Sarf tüketim tanımı bulunmuyor" AllowPaging="true" PageSize="3" OnPageIndexChanging="grdTanim_PageIndexChanging" OnRowCommand="grdTanim_RowCommand">

                                    <PagerStyle CssClass="pagination-ys" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDel"
                                                    runat="server"
                                                    CssClass="btn btn-danger"
                                                    CommandName="del" CommandArgument='<%#Eval("tanim_id") + ";" + Eval("tarifeid") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Masraf" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("masraf") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>

                                                <asp:LinkButton ID="btnTedarikci"
                                                    runat="server"
                                                    CssClass="btn btn-primary"
                                                    CommandName="detail" CommandArgument='<%#Eval("masraf_id") %>' Text=' <%#Eval("masraf") %> '>
                           
                                                </asp:LinkButton>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:BoundField DataField="masraf_saat" HeaderText="Birim Sarf"></asp:BoundField>
                                        <asp:BoundField DataField="birim" HeaderText="Birim"></asp:BoundField>
                                        <asp:BoundField DataField="tarife_kodu" HeaderText="Tarife"></asp:BoundField>

                                    </Columns>

                                </asp:GridView>

                                <asp:Button ID="btnyeniTanim" CssClass="btn btn-primary" Text="Yeni Tanım" runat="server" OnClick="btnyeniTanim_Click" />

                            </div>
                        </div>

                    </div>
                    <div id="Div8" runat="server" class="panel panel-info">
                        <div class="panel-heading">
                            <h2 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseAtama" class="collapsed">Makineye Atanmış Operatörler</a>
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


                                                <asp:BoundField DataField="id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
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

                            </div>
                        </div>

                    </div>
                    <div id="Div9" runat="server" class="panel panel-info">
                        <div class="panel-heading">
                            <h2 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseTarife" class="collapsed">Çalışma Tipine Göre Kiralama Tarifeleri</a>
                            </h2>
                        </div>
                        <div id="collapseTarife" class="panel-collapse" style="height: 0px;">
                            <div class="table-responsive">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdTarifeler" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="id" OnRowCommand="grdTarifeler_RowCommand"
                                            EmptyDataText="Tarife tanımı bulunmuyor"
                                            AllowPaging="true" PageSize="10">

                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnDelTarife"
                                                            runat="server"
                                                            CssClass="btn btn-danger"
                                                            CommandName="del" CommandArgument='<%# Eval("id") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'>Sil</i>" />

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                                <asp:BoundField DataField="ad" HeaderText="Tarife"></asp:BoundField>
                                                <asp:BoundField DataField="tarife_kodu" HeaderText="Tarife Kodu" />
                                                <asp:BoundField DataField="calisma_tipi" HeaderText="Çalışma" />
                                                <asp:BoundField DataField="tutar" HeaderText="Tutar" />

                                            </Columns>
                                        </asp:GridView>

                                    </ContentTemplate>
                                    <Triggers>
                                        <%--  <asp:AsyncPostBackTrigger ControlID="grdAtamalar" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnYeniTarife" EventName="Click" />--%>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <div class=" btn-group pull-right visible-lg">

                                    <asp:LinkButton ID="btnYeniTarife"
                                        runat="server"
                                        CssClass="btn btn-info" OnClick="btnYeniTarife_Click"
                                        Text="<i class='fa fa-plus icon-2x'>Yeni</i>" />

                                </div>

                            </div>
                        </div>

                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAra" EventName="ServerClick" />
                </Triggers>
            </asp:UpdatePanel>

        </div>

        <div id="atamaModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="atamaModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">

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
                                        <asp:Panel runat="server" ID="panel2" DefaultButton="btnOperatorAra">

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


                                                        <asp:BoundField DataField="userName" HeaderText="Operatör">
                                                            <HeaderStyle CssClass="visible-lg" />
                                                            <ItemStyle CssClass="visible-lg" />
                                                        </asp:BoundField>

                                                    </Columns>

                                                </asp:GridView>

                                                <div class="form-horizontal">

                                                    <div class="form-group">
                                                        <asp:Label runat="server" AssociatedControlID="chcHepsi" CssClass="col-md-2 control-label">Diğerlerini</asp:Label>
                                                        <div class="col-md-10">
                                                            <asp:CheckBox Text="Çıkar" ID="chcHepsi" CssClass="form-control" runat="server" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <div class="btn-group-justified">
                                                            <div class="col-md-10  pull-right">
                                                                <asp:Button ID="btnAtamaKaydet" runat="server" Text="Kaydet" CssClass="btn btn-info" OnClick="btnAtamaKaydet_Click" />
                                                                <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Kapat</button>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
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
        <%-- yeni sayac modal başlıyor --%>
        <div id="yeniSayacModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="detaySayacModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        ×</button>
                    <h3 id="detaySayacModalLabel" class="baslik">Servis Bakım Sayacı Kaydı</h3>
                </div>
                <div class="panel panel-info">

                    <%-- müşteri aramayı ekleyecez --%>
                    <div id="collapseSayac" class="panel-collapse in" style="height: auto;">
                        <div class="panel-body">
                            <!-- Müşteri seçim alanı başlıyor -->
                            <div class="table-responsive">

                                <asp:Panel runat="server" ID="panel1" DefaultButton="btnMasrafAra">
                                    <div class="input-group custom-search-form col-md-12">
                                        <input runat="server" type="text" id="txtMasrafAra" class="form-control" placeholder="Ara..." />
                                        <span class="input-group-btn">
                                            <%--<button id="btnMasrafAra2" runat="server" class="btn btn-default" type="submit" onserverclick="MasrafAra2">
                                            <i class="fa fa-search"></i>
                                        </button>--%>
                                            <asp:LinkButton ID="btnMasrafAra"
                                                runat="server"
                                                CssClass="btn btn-info" OnClick="MasrafAra"
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
                        <asp:GridView ID="grdMasraf" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="MasrafID"
                            EmptyDataText="Masraf kalemi seç" EnablePersistedSelection="true" OnSelectedIndexChanged="grdMasraf_SelectedIndexChanged">

                            <SelectedRowStyle CssClass="danger" />
                            <Columns>
                                <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-info" ButtonType="Button" Text="Seç" HeaderText="Seçim">
                                    <ControlStyle CssClass="btn btn-primary"></ControlStyle>
                                </asp:ButtonField>
                                <asp:BoundField DataField="MasrafID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="adi" HeaderText="Masraf" />

                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="birim" HeaderText="Birim" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>

                        <%-- tamircinin yapacağı işlem bilgileri --%>
                        <div class="panel panel-info">
                            <div id="collapseKarar" class="panel-collapse in" style="height: auto;">
                                <div class="panel-body">
                                    <div class="form-horizontal">

                                        <div class="form-group">
                                            <asp:Label runat="server" AssociatedControlID="txtSayac" CssClass="col-md-2 control-label">Sayaç(Saat)</asp:Label>
                                            <div class="col-md-10">
                                                <asp:TextBox runat="server" ID="txtSayac" TextMode="Number" ValidationGroup="detayGrupMasraf" CssClass="form-control" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtSayac" ValidationGroup="detayGrupMasraf" ErrorMessage="Lütfen sayı giriniz"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label runat="server" AssociatedControlID="txtSayacAlarm" CssClass="col-md-2 control-label">Alarm(Kaç saat kala)</asp:Label>
                                            <div class="col-md-10">
                                                <asp:TextBox runat="server" ID="txtSayacAlarm" TextMode="Number" ValidationGroup="detayGrupMasraf" CssClass="form-control" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtSayacAlarm" ValidationGroup="detayGrupMasraf" ErrorMessage="Lütfen sayı giriniz"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <asp:HiddenField ID="hdnSayacID" runat="server" />
                                        <div class="form-group">
                                            <div class="btn-group-justified">
                                                <div class="col-md-10  pull-right">
                                                    <asp:Button ID="btnSayacKaydet" runat="server" Text="Kaydet" CausesValidation="true" ValidationGroup="valGrupTamir" CssClass="btn btn-info" OnClick="btnSayacKaydet_Click" />
                                                    <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Kapat</button>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdMasraf" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnMasrafAra" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <%--  yeni sayac modal bitiyor--%>

        <div id="yeniTanimModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="yeniTanimModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        ×</button>
                    <h3 id="yeniTanimModalLabel" class="baslik">Saatlik Sarf Tüketim Kaydı</h3>
                </div>
                <div class="panel panel-info">

                    <%-- müşteri aramayı ekleyecez --%>
                    <div id="collapseTanim2" class="panel-collapse in" style="height: auto;">
                        <div class="panel-body">
                            <!-- Müşteri seçim alanı başlıyor -->
                            <div class="table-responsive">
                                <asp:Panel runat="server" ID="panelTamirci" DefaultButton="btnMasrafAra2">
                                    <div class="input-group custom-search-form col-md-12">
                                        <input runat="server" type="text" id="txtMasrafAra2" class="form-control" placeholder="Ara..." />
                                        <span class="input-group-btn">
                                            <%--<button id="btnMasrafAra2" runat="server" class="btn btn-default" type="submit" onserverclick="MasrafAra2">
                                            <i class="fa fa-search"></i>
                                        </button>--%>
                                            <asp:LinkButton ID="btnMasrafAra2"
                                                runat="server"
                                                CssClass="btn btn-info" OnClick="MasrafAra2"
                                                Text="<i class='fa fa-search icon-2x'></i>" />
                                        </span>
                                    </div>
                                </asp:Panel>
                            </div>
                            <!-- Müşteri seç,malanı bitiyor-->
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdMasraf2" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="MasrafID"
                            EmptyDataText="Masraf kalemi arayın" EnablePersistedSelection="true" OnSelectedIndexChanged="grdMasraf2_SelectedIndexChanged">

                            <SelectedRowStyle CssClass="danger" />
                            <Columns>
                                <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-info" ButtonType="Button" Text="Seç" HeaderText="Seçim">
                                    <ControlStyle CssClass="btn btn-primary"></ControlStyle>
                                </asp:ButtonField>
                                <asp:BoundField DataField="MasrafID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="adi" HeaderText="Masraf" />

                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="birim" HeaderText="Birim" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>

                        <%-- tamircinin yapacağı işlem bilgileri --%>
                        <div class="panel panel-info">
                            <div id="collapseKarar2" class="panel-collapse in" style="height: auto;">
                                <div class="panel-body">
                                    <div class="form-horizontal">

                                        <div class="form-group">
                                            <asp:Label runat="server" AssociatedControlID="txtSayac2" CssClass="col-md-2 control-label">Kaç Saat Çalışma</asp:Label>
                                            <div class="col-md-10">
                                                <asp:TextBox runat="server" ID="txtSayac2" TextMode="Number" ValidationGroup="detayGrupMasraf2" CssClass="form-control" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSayac2" ValidationGroup="detayGrupMasraf2" ErrorMessage="Lütfen sayı giriniz"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label runat="server" AssociatedControlID="txtMiktar2" CssClass="col-md-2 control-label">NE kadar sarf</asp:Label>
                                            <div class="col-md-10">
                                                <asp:TextBox runat="server" ID="txtMiktar2" ValidationGroup="detayGrupMasraf2" CssClass="form-control" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMiktar2" ValidationGroup="detayGrupMasraf2" ErrorMessage="Lütfen sayı giriniz"></asp:RequiredFieldValidator>
                                                <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtMiktar2" Type="Currency" ValidationGroup="detayGrupMasraf2" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label runat="server" AssociatedControlID="txtAciklama" CssClass="col-md-2 control-label">Açıklama</asp:Label>
                                            <div class="col-md-10">
                                                <asp:TextBox runat="server" ID="txtAciklama" ValidationGroup="detayGrupMasraf2" CssClass="form-control" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2 control-label" for="drdTarifeTanim">Tarife</label>
                                            <div class="col-md-10">
                                                <asp:DropDownList ID="drdTarifeTanim" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <asp:HiddenField ID="hdnTanimID" runat="server" />
                                        <div class="form-group">
                                            <div class="btn-group-justified">
                                                <div class="col-md-10  pull-right">
                                                    <asp:Button ID="btnTanimKaydet" runat="server" Text="Kaydet" CausesValidation="true" ValidationGroup="detayGrupMasraf2" CssClass="btn btn-info" OnClick="btnTanimKaydet_Click" />
                                                    <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Kapat</button>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdMasraf2" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnMasrafAra2" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div id="addTarifeModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="addModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        ×</button>
                    <h3 id="addModalLabel" class="baslik">Yeni Tarife Tanımı</h3>
                </div>
                <asp:UpdatePanel ID="upAdd" runat="server">

                    <ContentTemplate>

                        <div class="modal-body">
                            <div class="form-horizontal">

                                <div class="form-group">
                                    <label class="col-md-4 control-label" for="drdTarifeTipi">Tarife</label>
                                    <div class="col-md-8">
                                        <asp:DropDownList ID="drdTarifeTipi" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Saatlik" Value="saat"></asp:ListItem>
                                            <asp:ListItem Text="Günlük" Value="gun"></asp:ListItem>
                                            <asp:ListItem Text="Haftalık" Value="hafta"></asp:ListItem>
                                            <asp:ListItem Text="Aylık" Value="ay"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtCalismaTipi" CssClass="col-md-4 control-label">Çalışma Tipi</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtCalismaTipi" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtCalismaTipi" ErrorMessage="Lütfen  çalışma tipi giriniz"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtTarifeFiyat" CssClass="col-md-4 control-label">Fiyat</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtTarifeFiyat" TextMode="Number" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtTarifeFiyat" ErrorMessage="Lütfen  fiyat giriniz"></asp:RequiredFieldValidator>
                                    </div>
                                </div>


                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnAddRecordTarife" runat="server" Text="Kaydet"
                                CssClass="btn btn-info" ValidationGroup="musteriGrup2" OnClick="btnAddRecordTarife_Click" />
                            <button class="btn btn-info" data-dismiss="modal"
                                aria-hidden="true">
                                Kapat</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAddRecordTarife" EventName="Click" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


    <script type="text/javascript">
        function pageLoad(sender, args) {
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
