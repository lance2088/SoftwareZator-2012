﻿Public Class Obtenir_Cellule_Excel_Form
    Inherits VelerSoftware.Plugins3.ActionForm

    Private Sub FonctionForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        With Me
            .CancelButtonText = RM.GetString("CancelButtonText")
            .Title = RM.GetString("DisplayName")
            .Help_File = RM.GetString("Help_File")

            .Hote_ActionTextBox1.Tools = .Tools

            .Hote_ActionTextBox1.Text = "A1"

            .Boutons_ComboBox.SelectedIndex = 0
            .Boutons_ComboBox.Items.Clear()
            .ComboBox1.SelectedIndex = 0
            .ComboBox1.Items.Clear()
            For Each a As VelerSoftware.SZVB.Projet.Variable In .Tools.GetCurrentProjectVariableList
                If Not a.Array Then
                    .Boutons_ComboBox.Items.Add(a.Name)
                    .ComboBox1.Items.Add(a.Name)
                End If
            Next

            If Not .Param1 = Nothing Then
                If Not .Boutons_ComboBox.FindString(.Param1) = -1 Then
                    .Boutons_ComboBox.Text = .Boutons_ComboBox.Items(.Boutons_ComboBox.FindString(.Param1))
                End If
            End If

            If Not .Param2 = Nothing Then
                .Hote_ActionTextBox1.Text = .Param2
            End If

            If Not .Param3 = Nothing Then
                If Not .ComboBox1.FindString(.Param3) = -1 Then
                    .ComboBox1.Text = .ComboBox1.Items(.ComboBox1.FindString(.Param3))
                End If
            End If

        End With
    End Sub

    Private Sub FonctionForm_OnOKButtonClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.OnOKButtonClicked
        With Me
            If .Boutons_ComboBox.Text = "" OrElse .Hote_ActionTextBox1.Text = "" OrElse .ComboBox1.Text = "" Then
                MsgBox(RM.GetString("Formulaire_Incomplet"), MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            .Param1 = .Boutons_ComboBox.Text
            .Param2 = .Hote_ActionTextBox1.Text
            .Param3 = .ComboBox1.Text

            .DialogResult = Windows.Forms.DialogResult.OK
            .Close()
        End With
    End Sub

    Private Sub FonctionForm_OnCancelButtonClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.OnCancelButtonClicked
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub FonctionForm_OnRefreshCodeButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.OnRefreshCodeButtonClick
        If Me.Boutons_ComboBox.Text = "" OrElse Me.Hote_ActionTextBox1.Text = "" OrElse Me.ComboBox1.Text = "" Then
            MsgBox(RM.GetString("Formulaire_Incomplet"), MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Dim sourceWriter As New IO.StringWriter()

        Dim OperationStatement As New CodeDom.CodeAssignStatement(New CodeDom.CodeVariableReferenceExpression(Me.ComboBox1.Text), New CodeDom.CodeMethodInvokeExpression(New CodeDom.CodeVariableReferenceExpression(Me.Boutons_ComboBox.Text), "GetCellValue", New CodeDom.CodeSnippetExpression(Me.Tools.TransformKeyVariables(Me.Hote_ActionTextBox1.Text, True))))
        CodeDom.Compiler.CodeDomProvider.CreateProvider("VB").GenerateCodeFromStatement(OperationStatement, sourceWriter, New CodeDom.Compiler.CodeGeneratorOptions())

        sourceWriter.Close()

        Me.CodeEditor_Text = sourceWriter.ToString
    End Sub

    Private Sub FonctionForm_OnParseCodeButtonClick(ByVal sender As CodeDom.CodeCompileUnit, ByVal e As System.EventArgs) Handles MyBase.OnParseCodeButtonClick
        If (Not sender Is Nothing) AndAlso (sender.Namespaces.Count > 0) Then
            Dim metho As CodeDom.CodeMethodInvokeExpression
            For Each sta As CodeDom.CodeStatement In DirectCast(sender.Namespaces(0).Types(0).Members(0), CodeDom.CodeMemberMethod).Statements

                If TypeOf sta Is CodeDom.CodeAssignStatement AndAlso TypeOf DirectCast(sta, CodeDom.CodeAssignStatement).Right Is CodeDom.CodeMethodInvokeExpression Then
                    With DirectCast(sta, CodeDom.CodeAssignStatement)
                        If TypeOf .Left Is CodeDom.CodeVariableReferenceExpression Then Me.ComboBox1.Text = DirectCast(.Left, CodeDom.CodeVariableReferenceExpression).VariableName
                    End With
                    ' Dans le cas où se soit une simple méthode
                    metho = DirectCast(DirectCast(sta, CodeDom.CodeAssignStatement).Right, CodeDom.CodeMethodInvokeExpression)
                    If TypeOf metho.Method.TargetObject Is CodeDom.CodeVariableReferenceExpression Then
                        Me.Boutons_ComboBox.Text = DirectCast(metho.Method.TargetObject, CodeDom.CodeVariableReferenceExpression).VariableName
                    End If
                    If metho.Method.MethodName = "GetCellValue" Then
                        If metho.Parameters.Count > 0 AndAlso TypeOf metho.Parameters(0) Is CodeDom.CodePrimitiveExpression Then
                            Me.Hote_ActionTextBox1.Text = CStr(DirectCast(metho.Parameters(0), CodeDom.CodePrimitiveExpression).Value)
                        End If
                    End If
                End If

            Next
        End If
    End Sub

End Class