<%@ Page Title="" Language="C#" MasterPageFile="~/TeknikOperator/Operator.Master" AutoEventWireup="true" CodeBehind="ServislerCanli.aspx.cs" Inherits="TeknikServis.TeknikOperator.ServislerCanli" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">

        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">

                <h4 id="baslikkk" runat="server" class="panel-title">
                    <label id="baslik" runat="server">Atandığınız Aktif Şantiyeler</label>
               
                </h4>
            </div>
            <%--<div class="panel-body">
               
            </div>--%>
            <div class="table-responsive ">
              
                <div class="input-group custom-search-form col-md-6">
                    <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ara..." />
                    <span class="input-group-btn">
                        <button id="btnARA" runat="server" class="btn btn-default" type="submit" onserverclick="MusteriAra">
                            <i class="fa fa-search"></i>
                        </button>
                    </span>
                </div>
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upCrudGrid">
                    <ProgressTemplate>

                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel ID="upCrudGrid" runat="server">
                    <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(jScript);
                        </script>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="serviceID"
                            EmptyDataText="Kayıt girilmemiş" OnRowCreated="GridView1_OnRowCreated"
                            AllowPaging="true" PageSize="50" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanging="GridView1_PageIndexChanging">
      
                            <RowStyle ForeColor="White" />
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>

            
                                <asp:TemplateField HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" HeaderStyle-Width="100">
                                    <ItemTemplate>

                                        <div class="visible-lg">
                                            <asp:LinkButton ID="btnServis"
                                                runat="server"
                                                CssClass="btn btn-danger btn-xs"
                                                Text="<i class='fa fa-wrench'></i>" />
                                     
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-lg" />

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Detay" HeaderStyle-CssClass="visible-xs visible-sm" ItemStyle-CssClass="visible-xs visible-sm">

                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnKucuk"
                                            runat="server"
                                            CssClass="btn btn-primary"
                                            Text="<i class='fa fa-refresh fa-spin'></i>" />

                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-xs visible-sm" />
                                </asp:TemplateField>

                                  <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri Adı" />

                                <asp:BoundField DataField="baslik" HeaderText="Konu" />

                           
                            </Columns>

                        </asp:GridView>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnARA" EventName="ServerClick" />
                    </Triggers>
                </asp:UpdatePanel>
 
            </div>
            <div class="panel-footer pull-right">
              
            </div>

        </div>

    </div>
</asp:Content>
