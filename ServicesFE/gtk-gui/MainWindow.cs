
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;
	
	private global::Gtk.Action FileAction;
	
	private global::Gtk.Action ExitAction;
	
	private global::Gtk.VBox vbox1;
	
	private global::Gtk.MenuBar menubar1;
	
	private global::Gtk.Notebook nbTabs;
	
	private global::Gtk.VBox vbox2;
	
	private global::Gtk.HBox hbox2;
	
	private global::Gtk.Label label3;
	
	private global::Gtk.ComboBox cbTubes;
	
	private global::Gtk.Button button1;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow;
	
	private global::Gtk.TreeView statisticsTree;
	
	private global::Gtk.HBox hbox1;
	
	private global::Gtk.Button button2;
	
	private global::Gtk.Label label1;
	
	private global::Gtk.HBox hbox3;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow1;
	
	private global::Gtk.TreeView servicesTree;
	
	private global::Gtk.VBox vbox3;
	
	private global::Gtk.Button button3;
	
	private global::Gtk.Button buttonEditService;
	
	private global::Gtk.Label label2;
	
	private global::Gtk.HBox hbox4;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow2;
	
	private global::Gtk.TreeView thirdPartiesTree;
	
	private global::Gtk.VBox vbox4;
	
	private global::Gtk.Button button13;
	
	private global::Gtk.Button buttonEditThirdParty;
	
	private global::Gtk.Button buttonShowAccountMappings;
	
	private global::Gtk.Label label4;
	
	private global::Gtk.HBox hbox5;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow3;
	
	private global::Gtk.TreeView publicKeysTree;
	
	private global::Gtk.VBox vbox5;
	
	private global::Gtk.Button button199;
	
	private global::Gtk.Button buttonEditPublicKey;
	
	private global::Gtk.Button buttonDeletePublicKey;
	
	private global::Gtk.Label label8;
	
	private global::Gtk.VBox vbox6;
	
	private global::Gtk.Table table1;
	
	private global::Gtk.ComboBox cbServices;
	
	private global::Gtk.ComboBox cbThirdParties;
	
	private global::Gtk.Label label6;
	
	private global::Gtk.Label label7;
	
	private global::Gtk.HBox hbox6;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow4;
	
	private global::Gtk.TreeView serviceDefinitionsTree;
	
	private global::Gtk.VBox vbox7;
	
	private global::Gtk.Button button313;
	
	private global::Gtk.Button buttonEditServiceDefinition;
	
	private global::Gtk.Button buttonDeleteServiceDefinition;
	
	private global::Gtk.Label label5;
	
	private global::Gtk.Statusbar statusbar1;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager ();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
		this.FileAction = new global::Gtk.Action ("FileAction", global::Mono.Unix.Catalog.GetString ("File"), null, null);
		this.FileAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("File");
		w1.Add (this.FileAction, null);
		this.ExitAction = new global::Gtk.Action ("ExitAction", global::Mono.Unix.Catalog.GetString ("Exit"), null, null);
		this.ExitAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Exit");
		w1.Add (this.ExitAction, "<Primary>q");
		this.UIManager.InsertActionGroup (w1, 0);
		this.AddAccelGroup (this.UIManager.AccelGroup);
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString ("<ui><menubar name='menubar1'><menu name='FileAction' action='FileAction'><menuitem name='ExitAction' action='ExitAction'/></menu></menubar></ui>");
		this.menubar1 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menubar1")));
		this.menubar1.Name = "menubar1";
		this.vbox1.Add (this.menubar1);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.menubar1]));
		w2.Position = 0;
		w2.Expand = false;
		w2.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.nbTabs = new global::Gtk.Notebook ();
		this.nbTabs.CanFocus = true;
		this.nbTabs.Name = "nbTabs";
		this.nbTabs.CurrentPage = 2;
		// Container child nbTabs.Gtk.Notebook+NotebookChild
		this.vbox2 = new global::Gtk.VBox ();
		this.vbox2.Name = "vbox2";
		this.vbox2.Spacing = 6;
		// Container child vbox2.Gtk.Box+BoxChild
		this.hbox2 = new global::Gtk.HBox ();
		this.hbox2.Name = "hbox2";
		this.hbox2.Spacing = 6;
		this.hbox2.BorderWidth = ((uint)(6));
		// Container child hbox2.Gtk.Box+BoxChild
		this.label3 = new global::Gtk.Label ();
		this.label3.Name = "label3";
		this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Tubes");
		this.hbox2.Add (this.label3);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.label3]));
		w3.Position = 0;
		w3.Expand = false;
		w3.Fill = false;
		// Container child hbox2.Gtk.Box+BoxChild
		this.cbTubes = global::Gtk.ComboBox.NewText ();
		this.cbTubes.Name = "cbTubes";
		this.hbox2.Add (this.cbTubes);
		global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.cbTubes]));
		w4.Position = 1;
		// Container child hbox2.Gtk.Box+BoxChild
		this.button1 = new global::Gtk.Button ();
		this.button1.CanFocus = true;
		this.button1.Name = "button1";
		this.button1.UseStock = true;
		this.button1.UseUnderline = true;
		this.button1.Label = "gtk-refresh";
		this.hbox2.Add (this.button1);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.button1]));
		w5.PackType = ((global::Gtk.PackType)(1));
		w5.Position = 2;
		w5.Expand = false;
		w5.Fill = false;
		this.vbox2.Add (this.hbox2);
		global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox2]));
		w6.Position = 0;
		w6.Expand = false;
		w6.Fill = false;
		// Container child vbox2.Gtk.Box+BoxChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.statisticsTree = new global::Gtk.TreeView ();
		this.statisticsTree.CanFocus = true;
		this.statisticsTree.Name = "statisticsTree";
		this.GtkScrolledWindow.Add (this.statisticsTree);
		this.vbox2.Add (this.GtkScrolledWindow);
		global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.GtkScrolledWindow]));
		w8.Position = 1;
		// Container child vbox2.Gtk.Box+BoxChild
		this.hbox1 = new global::Gtk.HBox ();
		this.hbox1.Name = "hbox1";
		this.hbox1.Spacing = 6;
		// Container child hbox1.Gtk.Box+BoxChild
		this.button2 = new global::Gtk.Button ();
		this.button2.CanFocus = true;
		this.button2.Name = "button2";
		this.button2.UseStock = true;
		this.button2.UseUnderline = true;
		this.button2.Label = "gtk-execute";
		this.hbox1.Add (this.button2);
		global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.button2]));
		w9.Position = 2;
		w9.Expand = false;
		w9.Fill = false;
		this.vbox2.Add (this.hbox1);
		global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox1]));
		w10.Position = 2;
		w10.Expand = false;
		w10.Fill = false;
		this.nbTabs.Add (this.vbox2);
		// Notebook tab
		this.label1 = new global::Gtk.Label ();
		this.label1.Name = "label1";
		this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Beanstalk");
		this.nbTabs.SetTabLabel (this.vbox2, this.label1);
		this.label1.ShowAll ();
		// Container child nbTabs.Gtk.Notebook+NotebookChild
		this.hbox3 = new global::Gtk.HBox ();
		this.hbox3.Name = "hbox3";
		this.hbox3.Spacing = 6;
		// Container child hbox3.Gtk.Box+BoxChild
		this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
		this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
		this.servicesTree = new global::Gtk.TreeView ();
		this.servicesTree.CanFocus = true;
		this.servicesTree.Name = "servicesTree";
		this.GtkScrolledWindow1.Add (this.servicesTree);
		this.hbox3.Add (this.GtkScrolledWindow1);
		global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.GtkScrolledWindow1]));
		w13.Position = 0;
		// Container child hbox3.Gtk.Box+BoxChild
		this.vbox3 = new global::Gtk.VBox ();
		this.vbox3.Name = "vbox3";
		this.vbox3.Spacing = 6;
		this.vbox3.BorderWidth = ((uint)(6));
		// Container child vbox3.Gtk.Box+BoxChild
		this.button3 = new global::Gtk.Button ();
		this.button3.CanFocus = true;
		this.button3.Name = "button3";
		this.button3.UseStock = true;
		this.button3.UseUnderline = true;
		this.button3.Label = "gtk-new";
		this.vbox3.Add (this.button3);
		global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.button3]));
		w14.Position = 0;
		w14.Expand = false;
		w14.Fill = false;
		// Container child vbox3.Gtk.Box+BoxChild
		this.buttonEditService = new global::Gtk.Button ();
		this.buttonEditService.Sensitive = false;
		this.buttonEditService.CanFocus = true;
		this.buttonEditService.Name = "buttonEditService";
		this.buttonEditService.UseStock = true;
		this.buttonEditService.UseUnderline = true;
		this.buttonEditService.Label = "gtk-edit";
		this.vbox3.Add (this.buttonEditService);
		global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.buttonEditService]));
		w15.Position = 1;
		w15.Expand = false;
		w15.Fill = false;
		this.hbox3.Add (this.vbox3);
		global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.vbox3]));
		w16.Position = 2;
		w16.Expand = false;
		w16.Fill = false;
		this.nbTabs.Add (this.hbox3);
		global::Gtk.Notebook.NotebookChild w17 = ((global::Gtk.Notebook.NotebookChild)(this.nbTabs [this.hbox3]));
		w17.Position = 1;
		// Notebook tab
		this.label2 = new global::Gtk.Label ();
		this.label2.Name = "label2";
		this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Services");
		this.nbTabs.SetTabLabel (this.hbox3, this.label2);
		this.label2.ShowAll ();
		// Container child nbTabs.Gtk.Notebook+NotebookChild
		this.hbox4 = new global::Gtk.HBox ();
		this.hbox4.Name = "hbox4";
		this.hbox4.Spacing = 6;
		// Container child hbox4.Gtk.Box+BoxChild
		this.GtkScrolledWindow2 = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow2.Name = "GtkScrolledWindow2";
		this.GtkScrolledWindow2.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow2.Gtk.Container+ContainerChild
		this.thirdPartiesTree = new global::Gtk.TreeView ();
		this.thirdPartiesTree.CanFocus = true;
		this.thirdPartiesTree.Name = "thirdPartiesTree";
		this.GtkScrolledWindow2.Add (this.thirdPartiesTree);
		this.hbox4.Add (this.GtkScrolledWindow2);
		global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.GtkScrolledWindow2]));
		w19.Position = 0;
		// Container child hbox4.Gtk.Box+BoxChild
		this.vbox4 = new global::Gtk.VBox ();
		this.vbox4.Name = "vbox4";
		this.vbox4.Spacing = 6;
		this.vbox4.BorderWidth = ((uint)(6));
		// Container child vbox4.Gtk.Box+BoxChild
		this.button13 = new global::Gtk.Button ();
		this.button13.CanFocus = true;
		this.button13.Name = "button13";
		this.button13.UseStock = true;
		this.button13.UseUnderline = true;
		this.button13.Label = "gtk-new";
		this.vbox4.Add (this.button13);
		global::Gtk.Box.BoxChild w20 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.button13]));
		w20.Position = 0;
		w20.Expand = false;
		w20.Fill = false;
		// Container child vbox4.Gtk.Box+BoxChild
		this.buttonEditThirdParty = new global::Gtk.Button ();
		this.buttonEditThirdParty.Sensitive = false;
		this.buttonEditThirdParty.CanFocus = true;
		this.buttonEditThirdParty.Name = "buttonEditThirdParty";
		this.buttonEditThirdParty.UseStock = true;
		this.buttonEditThirdParty.UseUnderline = true;
		this.buttonEditThirdParty.Label = "gtk-edit";
		this.vbox4.Add (this.buttonEditThirdParty);
		global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.buttonEditThirdParty]));
		w21.Position = 1;
		w21.Expand = false;
		w21.Fill = false;
		// Container child vbox4.Gtk.Box+BoxChild
		this.buttonShowAccountMappings = new global::Gtk.Button ();
		this.buttonShowAccountMappings.Sensitive = false;
		this.buttonShowAccountMappings.CanFocus = true;
		this.buttonShowAccountMappings.Name = "buttonShowAccountMappings";
		this.buttonShowAccountMappings.UseUnderline = true;
		this.buttonShowAccountMappings.Label = global::Mono.Unix.Catalog.GetString ("Account Mappings");
		this.vbox4.Add (this.buttonShowAccountMappings);
		global::Gtk.Box.BoxChild w22 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.buttonShowAccountMappings]));
		w22.Position = 2;
		w22.Expand = false;
		w22.Fill = false;
		this.hbox4.Add (this.vbox4);
		global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.vbox4]));
		w23.Position = 2;
		w23.Expand = false;
		w23.Fill = false;
		this.nbTabs.Add (this.hbox4);
		global::Gtk.Notebook.NotebookChild w24 = ((global::Gtk.Notebook.NotebookChild)(this.nbTabs [this.hbox4]));
		w24.Position = 2;
		// Notebook tab
		this.label4 = new global::Gtk.Label ();
		this.label4.Name = "label4";
		this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("Third Parties");
		this.nbTabs.SetTabLabel (this.hbox4, this.label4);
		this.label4.ShowAll ();
		// Container child nbTabs.Gtk.Notebook+NotebookChild
		this.hbox5 = new global::Gtk.HBox ();
		this.hbox5.Name = "hbox5";
		this.hbox5.Spacing = 6;
		// Container child hbox5.Gtk.Box+BoxChild
		this.GtkScrolledWindow3 = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow3.Name = "GtkScrolledWindow3";
		this.GtkScrolledWindow3.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow3.Gtk.Container+ContainerChild
		this.publicKeysTree = new global::Gtk.TreeView ();
		this.publicKeysTree.CanFocus = true;
		this.publicKeysTree.Name = "publicKeysTree";
		this.GtkScrolledWindow3.Add (this.publicKeysTree);
		this.hbox5.Add (this.GtkScrolledWindow3);
		global::Gtk.Box.BoxChild w26 = ((global::Gtk.Box.BoxChild)(this.hbox5 [this.GtkScrolledWindow3]));
		w26.Position = 0;
		// Container child hbox5.Gtk.Box+BoxChild
		this.vbox5 = new global::Gtk.VBox ();
		this.vbox5.Name = "vbox5";
		this.vbox5.Spacing = 6;
		// Container child vbox5.Gtk.Box+BoxChild
		this.button199 = new global::Gtk.Button ();
		this.button199.CanFocus = true;
		this.button199.Name = "button199";
		this.button199.UseStock = true;
		this.button199.UseUnderline = true;
		this.button199.Label = "gtk-new";
		this.vbox5.Add (this.button199);
		global::Gtk.Box.BoxChild w27 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.button199]));
		w27.Position = 0;
		w27.Expand = false;
		w27.Fill = false;
		// Container child vbox5.Gtk.Box+BoxChild
		this.buttonEditPublicKey = new global::Gtk.Button ();
		this.buttonEditPublicKey.Sensitive = false;
		this.buttonEditPublicKey.CanFocus = true;
		this.buttonEditPublicKey.Name = "buttonEditPublicKey";
		this.buttonEditPublicKey.UseStock = true;
		this.buttonEditPublicKey.UseUnderline = true;
		this.buttonEditPublicKey.Label = "gtk-edit";
		this.vbox5.Add (this.buttonEditPublicKey);
		global::Gtk.Box.BoxChild w28 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.buttonEditPublicKey]));
		w28.Position = 1;
		w28.Expand = false;
		w28.Fill = false;
		// Container child vbox5.Gtk.Box+BoxChild
		this.buttonDeletePublicKey = new global::Gtk.Button ();
		this.buttonDeletePublicKey.Sensitive = false;
		this.buttonDeletePublicKey.CanFocus = true;
		this.buttonDeletePublicKey.Name = "buttonDeletePublicKey";
		this.buttonDeletePublicKey.UseStock = true;
		this.buttonDeletePublicKey.UseUnderline = true;
		this.buttonDeletePublicKey.Label = "gtk-delete";
		this.vbox5.Add (this.buttonDeletePublicKey);
		global::Gtk.Box.BoxChild w29 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.buttonDeletePublicKey]));
		w29.Position = 2;
		w29.Expand = false;
		w29.Fill = false;
		this.hbox5.Add (this.vbox5);
		global::Gtk.Box.BoxChild w30 = ((global::Gtk.Box.BoxChild)(this.hbox5 [this.vbox5]));
		w30.Position = 2;
		w30.Expand = false;
		w30.Fill = false;
		this.nbTabs.Add (this.hbox5);
		global::Gtk.Notebook.NotebookChild w31 = ((global::Gtk.Notebook.NotebookChild)(this.nbTabs [this.hbox5]));
		w31.Position = 3;
		// Notebook tab
		this.label8 = new global::Gtk.Label ();
		this.label8.Name = "label8";
		this.label8.LabelProp = global::Mono.Unix.Catalog.GetString ("Public Keys");
		this.nbTabs.SetTabLabel (this.hbox5, this.label8);
		this.label8.ShowAll ();
		// Container child nbTabs.Gtk.Notebook+NotebookChild
		this.vbox6 = new global::Gtk.VBox ();
		this.vbox6.Name = "vbox6";
		this.vbox6.Spacing = 6;
		// Container child vbox6.Gtk.Box+BoxChild
		this.table1 = new global::Gtk.Table (((uint)(2)), ((uint)(2)), false);
		this.table1.Name = "table1";
		this.table1.RowSpacing = ((uint)(6));
		this.table1.ColumnSpacing = ((uint)(6));
		this.table1.BorderWidth = ((uint)(6));
		// Container child table1.Gtk.Table+TableChild
		this.cbServices = global::Gtk.ComboBox.NewText ();
		this.cbServices.Name = "cbServices";
		this.table1.Add (this.cbServices);
		global::Gtk.Table.TableChild w32 = ((global::Gtk.Table.TableChild)(this.table1 [this.cbServices]));
		w32.LeftAttach = ((uint)(1));
		w32.RightAttach = ((uint)(2));
		w32.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.cbThirdParties = global::Gtk.ComboBox.NewText ();
		this.cbThirdParties.Name = "cbThirdParties";
		this.table1.Add (this.cbThirdParties);
		global::Gtk.Table.TableChild w33 = ((global::Gtk.Table.TableChild)(this.table1 [this.cbThirdParties]));
		w33.TopAttach = ((uint)(1));
		w33.BottomAttach = ((uint)(2));
		w33.LeftAttach = ((uint)(1));
		w33.RightAttach = ((uint)(2));
		w33.XOptions = ((global::Gtk.AttachOptions)(4));
		w33.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.label6 = new global::Gtk.Label ();
		this.label6.Name = "label6";
		this.label6.LabelProp = global::Mono.Unix.Catalog.GetString ("Service:");
		this.table1.Add (this.label6);
		global::Gtk.Table.TableChild w34 = ((global::Gtk.Table.TableChild)(this.table1 [this.label6]));
		w34.XOptions = ((global::Gtk.AttachOptions)(4));
		w34.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.label7 = new global::Gtk.Label ();
		this.label7.Name = "label7";
		this.label7.LabelProp = global::Mono.Unix.Catalog.GetString ("Third Party:");
		this.table1.Add (this.label7);
		global::Gtk.Table.TableChild w35 = ((global::Gtk.Table.TableChild)(this.table1 [this.label7]));
		w35.TopAttach = ((uint)(1));
		w35.BottomAttach = ((uint)(2));
		w35.XOptions = ((global::Gtk.AttachOptions)(4));
		w35.YOptions = ((global::Gtk.AttachOptions)(4));
		this.vbox6.Add (this.table1);
		global::Gtk.Box.BoxChild w36 = ((global::Gtk.Box.BoxChild)(this.vbox6 [this.table1]));
		w36.Position = 0;
		w36.Expand = false;
		w36.Fill = false;
		// Container child vbox6.Gtk.Box+BoxChild
		this.hbox6 = new global::Gtk.HBox ();
		this.hbox6.Name = "hbox6";
		this.hbox6.Spacing = 6;
		// Container child hbox6.Gtk.Box+BoxChild
		this.GtkScrolledWindow4 = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow4.Name = "GtkScrolledWindow4";
		this.GtkScrolledWindow4.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow4.Gtk.Container+ContainerChild
		this.serviceDefinitionsTree = new global::Gtk.TreeView ();
		this.serviceDefinitionsTree.CanFocus = true;
		this.serviceDefinitionsTree.Name = "serviceDefinitionsTree";
		this.GtkScrolledWindow4.Add (this.serviceDefinitionsTree);
		this.hbox6.Add (this.GtkScrolledWindow4);
		global::Gtk.Box.BoxChild w38 = ((global::Gtk.Box.BoxChild)(this.hbox6 [this.GtkScrolledWindow4]));
		w38.Position = 0;
		// Container child hbox6.Gtk.Box+BoxChild
		this.vbox7 = new global::Gtk.VBox ();
		this.vbox7.Name = "vbox7";
		this.vbox7.Spacing = 6;
		this.vbox7.BorderWidth = ((uint)(6));
		// Container child vbox7.Gtk.Box+BoxChild
		this.button313 = new global::Gtk.Button ();
		this.button313.CanFocus = true;
		this.button313.Name = "button313";
		this.button313.UseStock = true;
		this.button313.UseUnderline = true;
		this.button313.Label = "gtk-new";
		this.vbox7.Add (this.button313);
		global::Gtk.Box.BoxChild w39 = ((global::Gtk.Box.BoxChild)(this.vbox7 [this.button313]));
		w39.Position = 0;
		w39.Expand = false;
		w39.Fill = false;
		// Container child vbox7.Gtk.Box+BoxChild
		this.buttonEditServiceDefinition = new global::Gtk.Button ();
		this.buttonEditServiceDefinition.Sensitive = false;
		this.buttonEditServiceDefinition.CanFocus = true;
		this.buttonEditServiceDefinition.Name = "buttonEditServiceDefinition";
		this.buttonEditServiceDefinition.UseStock = true;
		this.buttonEditServiceDefinition.UseUnderline = true;
		this.buttonEditServiceDefinition.Label = "gtk-edit";
		this.vbox7.Add (this.buttonEditServiceDefinition);
		global::Gtk.Box.BoxChild w40 = ((global::Gtk.Box.BoxChild)(this.vbox7 [this.buttonEditServiceDefinition]));
		w40.Position = 1;
		w40.Expand = false;
		w40.Fill = false;
		// Container child vbox7.Gtk.Box+BoxChild
		this.buttonDeleteServiceDefinition = new global::Gtk.Button ();
		this.buttonDeleteServiceDefinition.Sensitive = false;
		this.buttonDeleteServiceDefinition.CanFocus = true;
		this.buttonDeleteServiceDefinition.Name = "buttonDeleteServiceDefinition";
		this.buttonDeleteServiceDefinition.UseStock = true;
		this.buttonDeleteServiceDefinition.UseUnderline = true;
		this.buttonDeleteServiceDefinition.Label = "gtk-delete";
		this.vbox7.Add (this.buttonDeleteServiceDefinition);
		global::Gtk.Box.BoxChild w41 = ((global::Gtk.Box.BoxChild)(this.vbox7 [this.buttonDeleteServiceDefinition]));
		w41.Position = 2;
		w41.Expand = false;
		w41.Fill = false;
		this.hbox6.Add (this.vbox7);
		global::Gtk.Box.BoxChild w42 = ((global::Gtk.Box.BoxChild)(this.hbox6 [this.vbox7]));
		w42.Position = 2;
		w42.Expand = false;
		w42.Fill = false;
		this.vbox6.Add (this.hbox6);
		global::Gtk.Box.BoxChild w43 = ((global::Gtk.Box.BoxChild)(this.vbox6 [this.hbox6]));
		w43.Position = 1;
		this.nbTabs.Add (this.vbox6);
		global::Gtk.Notebook.NotebookChild w44 = ((global::Gtk.Notebook.NotebookChild)(this.nbTabs [this.vbox6]));
		w44.Position = 4;
		// Notebook tab
		this.label5 = new global::Gtk.Label ();
		this.label5.Name = "label5";
		this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("Service Definitions");
		this.nbTabs.SetTabLabel (this.vbox6, this.label5);
		this.label5.ShowAll ();
		this.vbox1.Add (this.nbTabs);
		global::Gtk.Box.BoxChild w45 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.nbTabs]));
		w45.Position = 1;
		// Container child vbox1.Gtk.Box+BoxChild
		this.statusbar1 = new global::Gtk.Statusbar ();
		this.statusbar1.Name = "statusbar1";
		this.statusbar1.Spacing = 6;
		this.vbox1.Add (this.statusbar1);
		global::Gtk.Box.BoxChild w46 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.statusbar1]));
		w46.Position = 2;
		w46.Expand = false;
		w46.Fill = false;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 595;
		this.DefaultHeight = 437;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.ExitAction.Activated += new global::System.EventHandler (this.OnExit);
		this.nbTabs.SwitchPage += new global::Gtk.SwitchPageHandler (this.OnSwitchPage);
		this.button1.Clicked += new global::System.EventHandler (this.OnRefreshTubes);
		this.button2.Clicked += new global::System.EventHandler (this.OnGetStatistics);
		this.servicesTree.CursorChanged += new global::System.EventHandler (this.OnServiceCursorChanged);
		this.button3.Clicked += new global::System.EventHandler (this.OnNewService);
		this.buttonEditService.Clicked += new global::System.EventHandler (this.OnEditService);
		this.thirdPartiesTree.CursorChanged += new global::System.EventHandler (this.OnThirdPartyCursorChanged);
		this.button13.Clicked += new global::System.EventHandler (this.OnNewThirdParty);
		this.buttonEditThirdParty.Clicked += new global::System.EventHandler (this.OnEditThirdParty);
		this.buttonShowAccountMappings.Clicked += new global::System.EventHandler (this.OnClickShowAccountMappings);
		this.publicKeysTree.CursorChanged += new global::System.EventHandler (this.OnPublicKeyCursorChange);
		this.button199.Clicked += new global::System.EventHandler (this.OnNewPublicKey);
		this.buttonEditPublicKey.Clicked += new global::System.EventHandler (this.OnEditPublicKey);
		this.buttonDeletePublicKey.Clicked += new global::System.EventHandler (this.OnDeletePublicKey);
		this.cbThirdParties.Changed += new global::System.EventHandler (this.OnSdThirdPartyChanged);
		this.cbServices.Changed += new global::System.EventHandler (this.OnSdServiceChanged);
		this.serviceDefinitionsTree.CursorChanged += new global::System.EventHandler (this.OnServiceDefinitionCursorChanged);
		this.button313.Clicked += new global::System.EventHandler (this.OnNewServiceDefinition);
		this.buttonEditServiceDefinition.Clicked += new global::System.EventHandler (this.OnEditServiceDefinition);
		this.buttonDeleteServiceDefinition.Clicked += new global::System.EventHandler (this.OnDeleteServiceDefinition);
	}
}
