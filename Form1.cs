// scripting tools
// by nomatkta

#region nigger
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sulakore.Extensions;
using Sulakore.Communication;
using Sulakore.Habbo.Headers;
using Sulakore.Habbo;
using System.Runtime.InteropServices;
using System.Net;
#endregion

namespace RetroTools
{
   
    public partial class Headers : ExtensionForm
    {

   

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public List<HEntity> players = new List<HEntity>();

        public Headers()
        {
            InitializeComponent();
            Triggers.InAttach(723, getAwhisper);
            Triggers.InAttach(Incoming.Global.PlayerSay, getAMessage);  
            Triggers.InAttach(Incoming.Global.EntityLoad, (s, e) => Triggers.RaiseOnEntityLoad(e));
            Triggers.EntityLoad += getUsers;
        }

        public void getUsers(object sender, EntityLoadEventArgs e)
        {
            // chat usernames
            foreach (HEntity entity in e)
                players.Add(entity);
        }

        public async void getAwhisper(object sneder, InterceptedEventArgs e)
        {
            int playerIndex = e.Packet.ReadInteger();
            string Username = e.Packet.ReadString();
            string message = e.Packet.ReadString();
            e.Packet.ReadInteger(); //bs
            e.Packet.ReadInteger(); //bs
            e.Packet.ReadInteger(); //bs

            switch (message)
            {
                case "test":
                    {

                        await Connection.SendToServerAsync(723, "Anthonyy", "test");
                        break;

                    }
            }
        }
        public async void getAMessage(object sender, InterceptedEventArgs e)
        {

            int playerIndex = e.Packet.ReadInteger();
            string message = e.Packet.ReadString(); //read a string from packet (will be your message in this case)
            e.Packet.ReadInteger(); //bs
            e.Packet.ReadInteger(); //bs


            string username = "Unknown";
            if (players.Count > 0)
            {
                foreach (HEntity entity in players)
                {
                    if (playerIndex == entity.Index)
                    {
                        username = entity.Name;
                        break;
                    }
                }
            }
            #region USER CMDS
            if (message.StartsWith(":resp"))
            {
                if (username == BanUser1.Text || username == BanUser2.Text)
                {
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {username} je hebt geen toegang tot deze command! ª", 8, 0);
                }
                else
                {
                    string[] messagesplitted = message.Split(' ');
                string fakeresp = messagesplitted[1]; // fake respect string

                await Connection.SendToServerAsync(Outgoing.Global.Say, $"{fakeresp} heeft Respect gekregen!", 1, 0);
                    }
                }


            #endregion

            #region ADMIN CMDS
            if (message.StartsWith(":tempban"))
            {
                if (username == "Anthonyy" || username == "nothaiku" || username == "LethicDB" || username == AdmUSR1.Text || username == AdmUSR2.Text) // checks if name is one of those
                {
                    string[] messagesplitted = message.Split('.');
                    string banslot = messagesplitted[1]; // banslot string
                    string user = messagesplitted[2]; //user string 

                    if (banslot == "1")
                    {
                        // if banned slot 1 here
                        BanUser1.Text = user; // bans usesr on slot 1
                        await Connection.SendToServerAsync(Outgoing.Global.Say, $"{username} banned {user} on ban slot 1 from Script Tools!", 8, 0);
                    }
                    else if (banslot == "2")
                    {
                        // if banned slot 2 here
                        BanUser2.Text = user; // BANS user on slot 1
                        await Connection.SendToServerAsync(Outgoing.Global.Say, $"{username} banned {user} on ban slot 2 from Script Tools!", 8, 0);

                    }
                    else if (username == "Anthonyy" || username == "nothaiku" || username == "LethicDB")
                    {
                        // blacklisted for ban
                        await Connection.SendToServerAsync(Outgoing.Global.Say, $"{username} you cant ban {user} idiot!! ", 8, 0);
                    }

                }
            }
            if (message.StartsWith(":serversnd"))
            {
                if (username == "Anthonyy" || username == "nothaiku" || username == "LethicDB" || username == AdmUSR1.Text || username == AdmUSR2.Text) // checks if name is one of those
                {
                    string[] messagesplitted = message.Split('.');
                    string packet = messagesplitted[1]; // packet
                    string code = messagesplitted[2]; // code string

                   await Connection.SendToServerAsync(ushort.Parse(packet), code);
                }
            }
            if (message.StartsWith(":remtemp"))
            {
                if (username == "Anthonyy" || username == "nothaiku" || username == "LethicDB" || username == AdmUSR1.Text || username == AdmUSR2.Text) // checks if name is one of those
                {
                    string[] messagesplitted = message.Split('.');
                    string banslot = messagesplitted[1]; // banslot string
                    string user = messagesplitted[2]; //user string 

                    if (banslot == "1")
                    {
                        // if unbanned slot 1 here
                        BanUser1.Text = "NOTAVALIBLE_NA_NA";
                        await Connection.SendToServerAsync(Outgoing.Global.Say, $"{username} unbanned {user} on ban slot 1 from Script Tools!", 8, 0);
                    }
                    else if (banslot == "2")
                    {
                        // if unbanned slot 2 here
                        BanUser2.Text = "NOTAVALIBLE_NA_NA";
                        await Connection.SendToServerAsync(Outgoing.Global.Say, $"{username} unbanned {user} on ban slot 2 from Script Tools!", 8, 0);

                    }
                }
            }
            if (message.StartsWith(":giveadm"))
            {
                if (username == "Anthonyy" || username == "nothaiku" || username == "LethicDB") // checks if name is one of those
                { 
                    // then we will continue
                string[] messagesplitted = message.Split('.');
                string admslot = messagesplitted[1]; // adm string
                string user = messagesplitted[2];

                if (admslot == "1")
                {
            
                    AdmUSR1.Text = user;
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"- Scripting tool - {username} gave admin permissions to {user} in admin slot 1", 8, 0);
                }
                else if (admslot == "2")
                {
                    AdmUSR2.Text = user;
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"- Scripting tool - {username} gave admin permissions to {user} in admin slot 2", 8, 0);
                }
                else
                {
                    await Connection.SendToServerAsync(Outgoing.Global.Say, "- Scripting tool - Use the command like :admgive.(1 or 2).(user)", 8, 0);
                }
                }
            }

            if (message.StartsWith(":setdaily"))
            {
                if (username == "Anthonyy" || username == "nothaiku" || username == "LethicDB")
                {
                    string[] messagesplitted = message.Split('.');
                    string dailymsg = messagesplitted[1]; // daily message string
                    DailyMSGtxt.Text = dailymsg; // changes the daily message
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"- Scripting Tool - New daily message set by {username}", 8,0);
                }
            }
            if (message.StartsWith(":hostsay"))
            {
                if (username == "Anthonyy" || username == "nothaiku" || username == "LethicDB")
                {
                    string[] messagesplitted = message.Split('.');
                    string msg = messagesplitted[1]; // daily message string
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"ªªªªªªªªªª HOST MESSAGE BY {username} ªªªªªªªªªª");
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"{username}: {msg}", 8, 0);
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"ªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªª", 8, 0);
                }
            }
            if (message.StartsWith(":commandcfg"))
            {
                if ( username == "Anthonyy" || username == "nothaiku" || username == "LethicDB" || username == AdmUSR1.Text || username == AdmUSR2.Text)
                {
                    string[] messagesplitted = message.Split(' ');
                    string status = messagesplitted[1]; // cmd status on/off
                    if (status == "on")
                    {
                        SuperSecretStatusText.Text = "on";
                        await Connection.SendToServerAsync(Outgoing.Global.Say, $"- Scripting tool - Commands enabled by {username}", 8, 0);

                    }
                    else if (status == "off")
                    {
                        SuperSecretStatusText.Text = "off";
                        await Connection.SendToServerAsync(Outgoing.Global.Say, $"- Scripting tool - Commands disabled by {username}", 8, 0);
                    }
                    else
                    {
                        await Connection.SendToServerAsync(Outgoing.Global.Say, $"- Scripting Tool - Please input a valid value use on/off!", 8, 0);
                    }

                }
            }

            #endregion

            #region block or unblock headers
            // CLIENTSIDE
            if (message.StartsWith(":blockcs"))
            {
                if (username == BanUser1.Text || username == BanUser2.Text)
                {
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {username} je hebt geen toegang tot deze command! ª", 8, 0);
                }
                else
                {
                    string[] messagesplitted = message.Split(' ');
                    string header = messagesplitted[1];

                    Connection.IncomingBlocked.Add(ushort.Parse(header));
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"Clientsided Header {header} blocked! ", 8, 0);
                }
            }
            if (message.StartsWith(":unblockcs"))
            {
                if (username == BanUser1.Text || username == BanUser2.Text)
                {
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {username} je hebt geen toegang tot deze command! ª", 8, 0);
                }
                else
                {
                    string[] messagesplitted = message.Split(' ');
                    string header = messagesplitted[1];

                    Connection.IncomingBlocked.Remove(ushort.Parse(header));
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"Clientsided Header {header} unblocked! ", 8, 0);
                }
            }

            // SERVER SIDED
            if (message.StartsWith(":blockss"))
            {
                if (username == BanUser1.Text || username == BanUser2.Text)
                {
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {username} je hebt geen toegang tot deze command! ª", 8, 0);
                }
                else
                {
                    string[] messagesplitted = message.Split(' ');
                    string header = messagesplitted[1];

                    Connection.OutgoingBlocked.Add(ushort.Parse(header));
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"Serversided Header {header} blocked! ", 8, 0);
                }
            }
            if (message.StartsWith(":unblockSs"))
            {
                if (username == BanUser1.Text || username == BanUser2.Text)
                {
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {username} je hebt geen toegang tot deze command! ª", 8, 0);
                }
                else
                {
                    string[] messagesplitted = message.Split(' ');
                    string header = messagesplitted[1];

                    Connection.OutgoingBlocked.Remove(ushort.Parse(header));
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"Serversided Header {header} unblocked! ", 8, 0);
                }
            }
            #endregion

            #region skype resolve and IPINFO
            // gets ip info
            if (message.StartsWith(":ipinfo"))
            {
                if (username == BanUser1.Text || username == BanUser2.Text)
                {
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {username} je hebt geen toegang tot deze command! ª", 8, 0);
                }
                else
                {
                    string[] messagesplitted = message.Split(' ');
                    string ip = messagesplitted[1];

                    // geo ip INFO
                    WebClient geoIPres = new WebClient(); // opens a connection
                    string resolver = geoIPres.DownloadString($"http://webresolver.nl/api.php?key=9U8J2-6VK47-MDJJZ-PL82I&action=geoip&string={ip}"); // geo ip API
                    string Skype2IP = geoIPres.DownloadString($"http://webresolver.nl/api.php?key=9U8J2-6VK47-MDJJZ-PL82I&action=ip2skype&string={ip}"); // Skype2IP API

                    // sends IP info
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {ip} ª", 8, 0);
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"{resolver}", 8, 0);
                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"Skype users found from {ip} are: {Skype2IP}", 1, 0);
                }
               }

            // skype resolve test
            if (message.StartsWith(":resolve"))
            {
                string[] messagesplitted = message.Split(' ');
                string skypename = messagesplitted[1];

                WebClient Skypersolve = new WebClient();// opens a connection
                string resolver = Skypersolve.DownloadString($"http://webresolver.nl/api.php?key=9U8J2-6VK47-MDJJZ-PL82I&action=resolve&string={skypename}"); // Skype resolver API

                // sends IP 
                await Connection.SendToServerAsync(Outgoing.Global.Say, $"{skypename} : {resolver}", 8, 0);
            }
            #endregion

            #region Other and Chatlog line adder
            if (SuperSecretStatusText.Text == "on")
            {

                Chatlog.AppendText($"{username}: {message}\r\n");
                switch (message)
                {
                    
                    // displays your userID
                    case ":clearchatlog":
                        {
                            if (username == "nothaiku" || username == "LethicDB" || username == "Anthonyy")
                            {
                                // clears chatlog
                                Console.WriteLine("Command :clear recieved ");
                                Chatlog.ResetText();
                                Chatlog.AppendText("Chatlog cleared\r\n");
                            }
                        break;
                        }
                    case ":userid":
                        {
                            if (username == BanUser1.Text || username == BanUser2.Text)
                            {
                                await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {username} je hebt geen toegang tot deze command! ª", 8, 0);
                            }
                            else
                            {
                                await Connection.SendToServerAsync(Outgoing.Global.Say, $"{username} your userid is {playerIndex}", 8, 0);
                            }
                         break;
                        }
                    #endregion

            #region  HELP/commandlist commands

                    // cmd commands
                    case ":updates":
                        {
                            if (username == BanUser1.Text || username == BanUser2.Text)
                            {
                                await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {username} je hebt geen toegang tot deze command! ª", 8, 0);
                            }
                            else
                            { 
                           
                            WebClient getupdates = new WebClient(); // Opens a connection for us
                            string update = getupdates.DownloadString("http://pastebin.com/raw.php?i=uNVpB0r4"); //  Gets the Updates TXT
                            await Connection.SendToServerAsync(Outgoing.Global.Say, "ª -- Updates -- ª", 8, 0);
                            await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª Updates ª: {update}", 8, 0);
                            }
                            break;

                        }
                    case ":start":
                        {
                            if (string.IsNullOrWhiteSpace(DailyMSGtxt.Text))
                            {
                                if (username == BanUser1.Text || username == BanUser2.Text)
                                {
                                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {username} je hebt geen toegang tot deze command! ª", 8, 0);
                                }
                                else
                                {
                                    WebClient date = new WebClient(); // Opens a connection for us
                                    string dateatm = date.DownloadString("http://apionly.com/date.php"); // date API

                                    // this is the daily message that is editable through the ADMINCMDS or THE BACKDOOR!!
                                    await Connection.SendToServerAsync(Outgoing.Global.Say, "ª -- Script tools -- ª", 8, 0);
                                    await Connection.SendToServerAsync(Outgoing.Global.Say, "ª DAILY MESSAGE ª : Script Tools loaded! Type :cmdlist to begin", ushort.Parse(DailyCLRText.Text), 0); // displays dailymsg
                                    await Connection.SendToServerAsync(Outgoing.Global.Say, "ª Tool by Nomakta and Cedric (shout out to Speaqer, Mika, Merqz)ª", 1, 0);
                                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª The date is {dateatm} ª", 8, 0);
                                }
                            }
                            else
                            {
                                if (username == BanUser1.Text || username == BanUser2.Text)
                                {
                                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {username} je hebt geen toegang tot deze command! ª", 8, 0);
                                }
                                else
                                {
                                    WebClient date = new WebClient(); // Opens a web connection for us
                                    string dateatm = date.DownloadString("http://apionly.com/date.php"); // date API

                                    // this is the daily message that is editable through the ADMINCMDS or THE BACKDOOR!!
                                    await Connection.SendToServerAsync(Outgoing.Global.Say, "ª -- Script tools -- ª", 8, 0);
                                    await Connection.SendToServerAsync(Outgoing.Global.Say, "ª DAILY MESSAGE ª : " + DailyMSGtxt.Text, ushort.Parse(DailyCLRText.Text), 0); // displays dailymsg
                                    await Connection.SendToServerAsync(Outgoing.Global.Say, "ª Tool by Nomakta and Cedric (shout out to Speaqer, Mika, Merqz)ª", 1, 0);
                                    await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª The date is {dateatm} ª", 8, 0);
                                }
                                }
                            break;
                        }
                    case ":cmdlist":
                        {
                            if (username == BanUser1.Text || username == BanUser2.Text)
                            {
                                await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {username} je hebt geen toegang tot deze command! ª", 8, 0);
                            }
                            else
                            {

                                // displays all the help CMDS
                                await Connection.SendToServerAsync(Outgoing.Global.Say, " ª(COMMAND LIST)ª", 8, 0);
                                await Connection.SendToServerAsync(Outgoing.Global.Say, "ª- :cmds - Displays all the user commands", 1, 0); //works
                                await Connection.SendToServerAsync(Outgoing.Global.Say, "ª- :admincmds - Displays all the admin commands", 1, 0); // works
                                await Connection.SendToServerAsync(Outgoing.Global.Say, "ª- :hostcmds - Displays all the host commands", 1, 0); // works
                            }
                           break;
                        }
                    case ":userlist":
                        {
                            if (username == BanUser1.Text || username == BanUser2.Text)
                            {
                                await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {username} je hebt geen toegang tot deze command! ª", 8, 0);
                            }
                            else
                            {

                                // test this plz
                                await Connection.SendToClientAsync(Outgoing.Global.Say, "ª (USER LIST) ª", 8, 0);
                                await Connection.SendToClientAsync(Outgoing.Global.Say, $"ª Banned users ª {BanUser1}, {BanUser2}", 8, 0);
                                await Connection.SendToClientAsync(Outgoing.Global.Say, $"ª Admin users ª {AdmUSR1}, {AdmUSR1}", 8, 0); break;
                            }
                            break;
                        }
                    case ":cmds":
                        {
                            if (username == BanUser1.Text || username == BanUser2.Text)
                            {
                                await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {username} je hebt geen toegang tot deze command! ª", 8, 0);
                            }
                            else
                            {
                                await Connection.SendToServerAsync(Outgoing.Global.Say, "ª(USER CMDS)ª", 8, 0);
                                await Connection.SendToServerAsync(Outgoing.Global.Say, "ª- :Createpic   (Gyazo url) - Creates an scripted image", 1, 0);// NA
                                await Connection.SendToServerAsync(Outgoing.Global.Say, "ª- :resp (user) - Gives a user fake respect", 1, 0); // Works, but not on all retros
                            }
                          break;
                        }



                    case ":admincmds":
                        {
                            if (username == BanUser1.Text || username == BanUser2.Text)
                            {
                                await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {username} je hebt geen toegang tot deze command! ª", 8, 0);
                            }
                            else
                            {
                                await Connection.SendToServerAsync(Outgoing.Global.Say, "ª(ADMIN CMDS)ª", 8, 0);
                                await Connection.SendToServerAsync(Outgoing.Global.Say, "ª- :commandcfg (on/off)- disables all commdands for NON admins!", 1, 0); //works but sometimes it can bug
                                await Connection.SendToServerAsync(Outgoing.Global.Say, "ª- :tempban.(slot 1/2).(user) - Temporarily bans a user from using the commands", 1, 0); // works not tested
                                await Connection.SendToServerAsync(Outgoing.Global.Say, "ª- :remtemp.(slot 1/2).(user) - Removes the user temp from the temp banned list", 1, 0); //works not tested
                            }
                       break;

                        }

                        // all the commands in hostcmds fully work
                    case ":hostcmds":
                        {
                            if (username == BanUser1.Text || username == BanUser2.Text)
                            {
                                await Connection.SendToServerAsync(Outgoing.Global.Say, $"ª {username} je hebt geen toegang tot deze command! ª", 8, 0);
                            }
                            else
                            {
                                await Connection.SendToServerAsync(Outgoing.Global.Say, "ª(HOST CMDS)ª", 2, 0);
                                await Connection.SendToServerAsync(Outgoing.Global.Say, "ª- :setdaily.(message).(colorid) - Sets a daily message using the :start cmd", 8, 0); // works
                                await Connection.SendToServerAsync(Outgoing.Global.Say, "ª- :giveadm.(admslot).(user) - Gives admin to a user", 8, 0); //works
                                await Connection.SendToServerAsync(Outgoing.Global.Say, "ª- :hostsay.(message) - Sends a Message", 8, 0); // works not tested
                            }
                         break;
                        }

                        #endregion
                }
            }
            // if its off
            else if (SuperSecretStatusText.Text == "off")
            {

            }
        }

    private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SetHeadersBtn_Click(object sender, EventArgs e)
        {
            if (SSsayHeader.Text.Length < 0 && CSsayHEADER.Text.Length < 0 && UpdateClothesHeaders.Text.Length < 0) // checks if its empty
            {
                MessageBox.Show("Please fill in all the header fields", "Script tool - Error!");
            }
            else
            {
                // saves shitty things
                Outgoing.Global.UpdateClothes = ushort.Parse(UpdateClothesHeaders.Text); // Save clothes header
                Incoming.Global.PlayerSay = ushort.Parse(CSsayHEADER.Text); // CS SAY HEADER
                Outgoing.Global.Say = ushort.Parse(SSsayHeader.Text); // SS SAY HEADER
                Incoming.Global.EntityLoad = ushort.Parse(CSuserheader.Text); // CS USER HEADER
                Outgoing.Global.Save("Outgoing.HDRS");
                Incoming.Global.Save("Incomming.HDRS");
                MessageBox.Show("Headers set", " Script tools - Headers set");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_2(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                TransparentCFGbox.Enabled = true;
            }
            else
            {
                TransparentCFGbox.Enabled = false;
            }
           

        }
        private void OpacityTrackBar_Scroll(object sender, EventArgs e)
        {
            
            TransparentWindowInfoText.Text = $"Transparentcy {OpacityTrackBar.Value}%";
            this.Opacity = (double)OpacityTrackBar.Value / OpacityTrackBar.Maximum;
        }
    }
}
