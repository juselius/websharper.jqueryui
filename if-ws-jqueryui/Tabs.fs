﻿// $begin{copyright}
//
// WebSharper examples
//
// Copyright (c) IntelliFactory, 2004-2009.
//
// All rights reserved.  Reproduction or use in whole or in part is
// prohibited without the written consent of the copyright holder.
//-----------------------------------------------------------------
// $end{copyright}

//JQueryUI Wrapping: (version Stable 1.8rc1)
namespace IntelliFactory.WebSharper.JQueryUI

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html



type TabsAjaxOptionsConfiguration =
    {
    [<Name "ajaxOptions">]
    async: bool
    }

    [<JavaScript>]
    static member Default = {async = false}


type TabsCookieConfiguration =
    {
    [<Name "cookie">]
    expires: int
    }

    [<JavaScript>]
    static member Default = {expires = 30}


type TabsFxConfiguration =
    {
    [<Name "fx">]
    opacity: string
    }
    [<JavaScript>]
    static member Dafault = {opacity = "toggle"}


type TabsInfo =
    {
        options : string []
        tab : Dom.Node
        panel : Dom.Node
        index : int
    }

type TabsConfiguration[<JavaScript>]() =

    [<DefaultValue>]
    //null by default
    val mutable ajaxOptions: TabsAjaxOptionsConfiguration

    [<DefaultValue>]
    //false by default
    val mutable cache: bool

    [<DefaultValue>]
    //false by default
    val mutable collapsible: bool

    [<DefaultValue>]
    //null by default
    val mutable cookie: TabsCookieConfiguration

    [<DefaultValue>]
    //false by default
    val mutable deselectable: bool

    [<DefaultValue>]
    //[] by default
    val mutable disabled: array<int>

    [<DefaultValue>]
    //"click" by default
    val mutable event: string

    //Option, Array?
    [<DefaultValue>]
    //null by default
    val mutable fx: TabsFxConfiguration

    [<DefaultValue>]
    //"ui-tabs-" by default
    val mutable idPrefix: string

    [<DefaultValue>]
    //"<div></div>" by default
    val mutable panelTemplate: string

    [<DefaultValue>]
    //0 by default
    val mutable selected: int

    [<DefaultValue>]
    //"<em>Loading&#8230;</em>" by default
    val mutable spinner: string

    [<DefaultValue>]
    //"<li><a href="#{href}"><span>#{label}</span></a></li>" by default
    val mutable tabTemplate: string

module internal TabsInternal =
    [<Inline "jQuery($el).tabs($conf)">]
    let Init(el: Dom.Element, conf: TabsConfiguration) = ()


[<Require(typeof<Dependencies.JQueryUIJs>)>]
[<Require(typeof<Dependencies.JQueryUICss>)>]
type Tabs[<JavaScript>] internal () =
    inherit Pagelet()

    (****************************************************************
    * Constructors
    *****************************************************************)
    /// Creates a new tabs object with panels and titles fromt the given
    /// list of name and element pairs and configuration settings object.
    [<JavaScript>]
    [<Name "New1">]
    static member New (els : List<string * Element>, conf: TabsConfiguration): Tabs =
        let el =
            let itemPanels =
                els
                |> List.map (fun (label, panel) ->
                   let id = NewId()
                   let item =
                    LI [
                        A [
                            Attr.HRef ("#" + id)
                            Text label
                        ]
                        panel
                    ]
                   let tab = Div [Attr.Id id] -< [panel]
                   (item :> IPagelet, tab :> IPagelet)
                )
            let ul =
                UL <| Seq.map fst itemPanels
            Div [ul] -< (List.map snd itemPanels)

        let tabs = new Tabs ()
        tabs.element <-
            el
            |>! OnAfterRender (fun el ->
                TabsInternal.Init(el.Body, conf)
            )
        tabs



    /// Creates a new tabs object using the default configuration.
    [<JavaScript>]
    [<Name "New2">]
    static member New (els : List<string * Element>): Tabs =
        Tabs.New(els, new TabsConfiguration())


    (****************************************************************
    * Methods
    *****************************************************************)
    /// Removes the tabs functionality completely.
    [<Inline "jQuery($this.element.Body).tabs('destroy')">]
    member this.Destroy() = ()

    /// Disables the tabs functionality.
    [<Inline "jQuery($this.element.Body).tabs('disable')">]
    member this.Disable () = ()

    /// Enables the tabs functionality.
    [<Inline "jQuery($this.element.Body).tabs('enable')">]
    member this.Enable () = ()

    /// Sets a tabs option.
    [<Inline "jQuery($this.element.Body).tabs('option', $name, $value)">]
    member this.Option (name: string, value: obj) = ()

    [<Inline "jQuery($this.element.Body).tabs('add', $url, $label, $index)">]
    member private this.add (url:string, label:string, index: int) = ()

    [<Inline "jQuery($this.element.Body).tabs('length')">]
    member private this.getLength () = 0

    /// Removes the tab with the given index.
    [<Inline "jQuery($this.element.Body).tabs('remove', $index)">]
    member this.Remove (index: int) = ()

    /// Selects the tab with the given index.
    [<Inline "jQuery($this.element.Body).tabs('select', $index)">]
    member this.Select (index: int) = ()

    /// Reloads the content of an Ajax tab.
    [<Inline "jQuery($this.element.Body).tabs('load', $index)">]
    member this.Load (index: int) = ()

    /// Changes the url from which an Ajax (remote) tab will be loaded.
    [<Inline "jQuery($this.element.Body).tabs('url', $index)">]
    member this.Url (index: int) = ()

    /// Sets up an automatic rotation through tabs of a tab pane.
    /// The second argument is an amount of time in milliseconds until the next
    /// tab in the cycle gets activated. Use 0 or null to stop the rotation.
    /// The third controls whether or not to continue the rotation after a tab has been
    /// selected by a user.
    [<Inline "jQuery($this.element.Body).tabs('rotate', $secs, $loop)">]
    member this.Rotate (secs: int, loop: bool) = ()

    /// Retrieve the number of tabs of the first matched tab pane.
    [<JavaScript>]
    member this.Length
        with get () = this.getLength()

    /// Add a new tab with the given element and label
    /// inserted at the specified index.
    [<JavaScript>]
    member this.Add(el: Element, label: string, ix: int) =
        let id = NewId()
        let tab = Div [Attr.Id id] -< [el]
        this.element.Append tab
        this.add("#" + id, label, ix)

    /// Add a new tab with the given element and label.
    [<JavaScript>]
    member this.Add(el: Element, label: string) =
        let id = NewId()
        let tab = Div [Attr.Id id] -< [el]
        this.element.Append tab
        this.add("#" + id, label, this.Length)

    (****************************************************************
    * Events
    *****************************************************************)
    [<Inline "jQuery($this.element.Body).tabs({select: function (x,y) {($f(x))(y);}})">]
    member private this.onSelect(f : JQuery.Event -> TabsInfo -> unit) = ()

    [<Inline "jQuery($this.element.Body).tabs({load: function (x,y) {$f(x);}})">]
    member private this.onLoad(f : JQuery.Event -> unit) = ()

    [<Inline "jQuery($this.element.Body).tabs({show: function (x,y) {$f(x);}})">]
    member private this.onShow(f : JQuery.Event -> unit) = ()

    [<Inline "jQuery($this.element.Body).tabs({add: function (x,y) {$f(x);}})">]
    member private this.onAdd(f : JQuery.Event -> unit) = ()

    [<Inline "jQuery($this.element.Body).tabs({remove: function (x,y) {$f(x);}})">]
    member private this.onRemove(f : JQuery.Event -> unit) = ()

    [<Inline "jQuery($this.element.Body).tabs({enable: function (x,y) {$f(x);}})">]
    member private this.onEnable(f : JQuery.Event -> unit) = ()

    [<Inline "jQuery($this.element.Body).tabs({diable: function (x,y) {$f(x);}})">]
    member private this.onDisable(f : JQuery.Event -> unit) = ()


    /// Event triggered when a tab is selcted.
    [<JavaScript>]
    member this.OnSelect f =
        this |> OnAfterRender(fun _ ->
            this.onSelect f
        )

    /// Event triggered when a tab is loaded.
    [<JavaScript>]
    member this.OnLoad f =
        this |> OnAfterRender(fun _ ->
            this.onLoad f
        )

    /// Event triggered when a tab is showed.
    [<JavaScript>]
    member this.OnShow f =
        this |> OnAfterRender(fun _  ->
            this.onShow f
        )

    /// Event triggered when a tab is added.
    [<JavaScript>]
    member this.OnAdd f =
        this |> OnAfterRender(fun _  ->
            this.onAdd f
        )
    /// Event triggered when a tab is enabled.
    [<JavaScript>]
    member this.OnEnable f =
        this |> OnAfterRender(fun _  ->
            this.onEnable f
        )
    /// Event triggered when a tab is disabled.
    [<JavaScript>]
    member this.OnDisable f =
        this |> OnAfterRender(fun _  ->
            this.onDisable f
        )