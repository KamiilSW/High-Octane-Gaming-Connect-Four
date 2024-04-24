
Public Class Form1

    Private Const GridSize As Integer = 8
    Private Grid As New List(Of List(Of Boolean?))() ' nullable boolean (blue:true, red:false, blank:nothing)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Panel1.BackColor = Color.Aqua

        ' initialize the grid to all blanks
        For col As Integer = 1 To GridSize
            Dim column As New List(Of Boolean?)
            For row As Integer = 1 To GridSize
                column.Add(Nothing)
            Next
            Grid.Add(column)
        Next
    End Sub

    Private Sub Panel1_SizeChanged(sender As Object, e As EventArgs) Handles Panel1.SizeChanged
        Dim pnl As Panel = DirectCast(sender, Panel)
        pnl.Invalidate() ' redraw the board whenever it gets resized
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        Dim pnl As Panel = DirectCast(sender, Panel)

        Dim p As Decimal
        Dim x, y As Integer
        Dim margin As Integer

        ' draw the vertical lines:
        margin = (1 / (GridSize + 1)) * pnl.Size.Height
        For col As Integer = 1 To GridSize
            p = col / (GridSize + 1)
            x = p * pnl.Size.Width
            e.Graphics.DrawLine(Pens.Black, x, margin, x, pnl.Size.Height - margin)
        Next

        ' draw the horizontal lines:
        margin = (1 / (GridSize + 1)) * pnl.Size.Width
        For row As Integer = 1 To GridSize
            p = row / (GridSize + 1)
            y = p * pnl.Size.Height
            e.Graphics.DrawLine(Pens.Black, margin, y, pnl.Size.Width - margin, y)
        Next

        ' draw the pieces:
        For x = 0 To GridSize - 1
            Dim column As List(Of Boolean?) = Grid(x)
            For y = 0 To GridSize - 1
                If Grid(x)(y).HasValue Then
                    Dim clr As Color = If(Grid(x)(y), Color.Blue, Color.Red)
                    Dim pt As New Point((x + 1) / (GridSize + 1) * pnl.Size.Width, (y + 1) / (GridSize + 1) * pnl.Size.Height)
                    Dim rc As New Rectangle(pt, New Size(1, 1))
                    rc.Inflate((1 / (GridSize + 1)) * pnl.Size.Width / 2, (1 / (GridSize + 1)) * pnl.Size.Height / 2)
                    Using brsh As New SolidBrush(clr)
                        e.Graphics.FillEllipse(brsh, rc)
                    End Using
                End If
            Next
        Next
    End Sub

    Private Sub Panel1_Click(sender As Object, e As EventArgs) Handles Panel1.Click
        Dim pnl As Panel = DirectCast(sender, Panel)

        ' figure out where the user clicked: min = 0, max = (gridsize -1)
        Dim pt As Point = pnl.PointToClient(Cursor.Position)
        Dim colWidth As Integer = (1 / (GridSize + 1)) * pnl.Size.Width
        Dim rowHeight As Integer = (1 / (GridSize + 1)) * pnl.Size.Height
        Dim gridPosition As New Point(Math.Min(Math.Max((pt.X / colWidth) - 1, 0), GridSize - 1), Math.Min(Math.Max((pt.Y / rowHeight) - 1, 0), GridSize - 1))

        'Now do something with gridPosition: (here we just toggle between blue:true, red:false and blank:nothing)
        'If Not Grid(gridPosition.X)(gridPosition.Y).HasValue Then
        '    Grid(gridPosition.X)(gridPosition.Y) = True
        'ElseIf Grid(gridPosition.X)(gridPosition.Y) = True Then
        '    Grid(gridPosition.X)(gridPosition.Y) = False
        'Else
        '    Grid(gridPosition.X)(gridPosition.Y) = Nothing
        'End If
        'pnl.Invalidate() ' force the board to redraw itself
    End Sub

End Class