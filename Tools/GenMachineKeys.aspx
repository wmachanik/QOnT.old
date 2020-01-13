<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="System.Security" %>
<%@ Import Namespace="System.Security.Cryptography" %>
<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">


    public string getRandomKey(int bytelength)
    {
        int len = bytelength * 2;
        byte[] buff = new byte[len / 2];
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        rng.GetBytes(buff);
        StringBuilder sb = new StringBuilder(bytelength * 2);
        for (int i = 0; i < buff.Length; i++)
            sb.Append(string.Format("{0:X2}", buff[i]));
        return sb.ToString();
    }

    public string getASPNET20machinekey()
    {
        StringBuilder aspnet20machinekey = new StringBuilder();
        string key64byte = getRandomKey(64);
        string key32byte = getRandomKey(32);
        aspnet20machinekey.Append("<machineKey \n");
        aspnet20machinekey.Append("validationKey=\"" + key64byte + "\"\n");
        aspnet20machinekey.Append("decryptionKey=\"" + key32byte + "\"\n");
        aspnet20machinekey.Append("validation=\"SHA1\" decryption=\"AES\"\n");
        aspnet20machinekey.Append("/>\n");
        return aspnet20machinekey.ToString();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        string _MachineKeys = getASPNET20machinekey();

        lblMachineKeys.Text = _MachineKeys;
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Access Auto class maker</title>
</head>
<body>
    <form id="frmAutoClassMaker" runat="server">
    <div>
      <asp:ScriptManager ID="smgrORMClass" runat="server">
      </asp:ScriptManager>
      <asp:UpdateProgress ID="uprgORMCLass" runat="server">
        <ProgressTemplate>updating...<img src="../images/animi/BlueArrowsUpdate.gif" alt="please wait" /></ProgressTemplate>
      </asp:UpdateProgress>
      <p>
        Generate Machine Keys from Server:</p>
		<asp:Button ID="btnGo" Text="Generate Keys" runat="server"  OnClick="btnGo_Click" />
		<br />
		Add this to Web config: <asp:Label ID="lblMachineKeys" runat="server" Text="press Go" />
    </div>
    </form>
</body>
</html>
