<%@ Control Language="C#" AutoEventWireup="true" CodeFile="KC-GroupList.ascx.cs" Inherits="RockWeb.Blocks.Utility.KC_GroupList" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

                    <Rock:Grid ID="gGroups" runat="server" AllowPaging="true" OnRowSelected="gGroups_RowSelected" DataKeyNames="Id">
                        <Columns>
                            <rock:RockBoundField DataField="Name" HeaderText="Group Name" />
                            <rock:RockBoundField DataField="GroupTypeName" HeaderText="Group Type" />
                            <rock:RockBoundField DataField="IsActive" HeaderText="Active" />
                        </Columns>
                    </Rock:Grid>
              
      
    </ContentTemplate>
</asp:UpdatePanel>
