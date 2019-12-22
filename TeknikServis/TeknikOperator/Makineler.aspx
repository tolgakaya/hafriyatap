<%@ Page Title="" Language="C#" MasterPageFile="~/TeknikOperator/Operator.Master" AutoEventWireup="true" CodeBehind="Makineler.aspx.cs" Inherits="TeknikServis.TeknikOperator.Makineler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">


        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                Atandığınız Makineler
            </div>
            <%--<div class="panel-body">
               
            </div>--%>
            <div class="table-responsive ">
                <div class="input-group custom-search-form">
                    <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ara..." />
                    <span class="input-group-btn">
                        <button id="btnARA" runat="server" class="btn btn-default" type="submit" onserverclick="MakineAra">
                            <i class="fa fa-search"></i>
                        </button>
                    </span>
                </div>
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel ID="upCrudGrid" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdAlimlar" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="makine_id"
                            EmptyDataText="Atandığınız bir makine yok"
                            AllowPaging="true" PageSize="10" OnPageIndexChanging="grdAlimlar_PageIndexChanging" OnRowCreated="grdAlimlar_RowCreated">

                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>

                                <asp:TemplateField HeaderStyle-Width="120">
                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnDetay"
                                            runat="server"
                                            CssClass="btn  btn-success"
                                            Text="<i class='fa fa-bar-chart'>Detay</i>" />

                                    </ItemTemplate>


                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Makine" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("adi") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnMakineAdi"
                                            runat="server"
                                            CssClass="btn btn-primary"
                                            Text=' <%#Eval("adi") %> '>
                           
                                        </asp:LinkButton>
                                    </ItemTemplate>

                                </asp:TemplateField>
 
                                <asp:BoundField DataField="plaka" HeaderText="Plaka"></asp:BoundField>
                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>

                            </Columns>

                        </asp:GridView>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAra" EventName="ServerClick" />

                    </Triggers>
                </asp:UpdatePanel>


            </div>


        </div>


    </div>
</asp:Content>
