Namespace Global.DungeonItems.Controls

    'https://stackoverflow.com/questions/8331940/how-can-i-get-a-listview-gridviewcolumn-to-fill-the-remaining-space-in-my-grid/47097984

    Public Class StarSizeHelper

        Private Shared ReadOnly KnownElements As New List(Of FrameworkElement)

        Public Shared ReadOnly IsEnabledProperty As DependencyProperty = DependencyProperty.RegisterAttached(
            "IsEnabled",
            GetType(Boolean),
            GetType(StarSizeHelper),
            New PropertyMetadata(False))

        Public Shared Function GetIsEnabled(d As DependencyObject) As Boolean
            Return d.GetValue(IsEnabledProperty)
        End Function

        Public Shared Sub IsEnabledChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim ctl As ListView = d
            If ctl Is Nothing Then
                Throw New Exception("IsEnabled attached property only works on a ListView type")
            End If
            RememberElement(ctl)
        End Sub

        Private Shared Sub RememberElement(ctl As ListView)
            If Not KnownElements.Contains(ctl) Then
                KnownElements.Add(ctl)
                RegisterEvents(ctl)
            End If
        End Sub

        Private Shared Sub OnUnloaded(sender As Object, e As RoutedEventArgs)
            Dim ctl As FrameworkElement = sender
            ForgetControl(ctl)
        End Sub

        Private Shared Sub ForgetControl(ctl As FrameworkElement)
            KnownElements.Remove(ctl)
            UnregisterEvents(ctl)
        End Sub

        Private Shared Sub RegisterEvents(ctl As FrameworkElement)
            AddHandler ctl.Unloaded, AddressOf OnUnloaded
            AddHandler ctl.SizeChanged, AddressOf OnSizeChanged
        End Sub

        Private Shared Sub UnregisterEvents(ctl As FrameworkElement)
            RemoveHandler ctl.Unloaded, AddressOf OnUnloaded
            RemoveHandler ctl.SizeChanged, AddressOf OnSizeChanged
        End Sub

        Private Shared Sub OnSizeChanged(sender As Object, e As SizeChangedEventArgs)
            Dim listView As ListView = sender
            If listView Is Nothing Then
                Return
            End If
            Dim workingWidth = listView.ActualWidth - 35
            listView.Width = workingWidth
        End Sub

    End Class

End Namespace
