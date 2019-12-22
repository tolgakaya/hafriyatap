<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Alarmlar.aspx.cs" Inherits="TeknikServis.Alarmlar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">
        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>


        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                <h4 id="baslikkk" runat="server" class="panel-title">
                    <label id="baslikk" runat="server">GÜNCEL UYARILAR</label>
                </h4>
            </div>

            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="table-responsive">

                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                            DataKeyNames="id"
                            EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand"
                            OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="true" PageSize="10">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="delLink"
                                            runat="server"
                                            CssClass="btn btn-danger btn-sm"
                                            CommandName="bag" CommandArgument='<%#Eval("baglanti") %>' Text="<i class='fa fa-check'></i>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="mesaj" HeaderText="Alarm" />
                            </Columns>
                        </asp:GridView>

                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:Timer ID="Timer1" runat="server" Interval="20000" OnTick="Timer1_Tick">
            </asp:Timer>

            <div class="panel-footer pull-right">
            </div>

        </div>
    </div>




</asp:Content>
