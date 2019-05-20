' Concede um BPC (87, 88)
' Riscos: Se tiver caractere invalido no CNIS

Rem Attribute VBA_ModuleType=VBAModule
Option VBASupport 1
Sub concede_BPCcomRL()

'Dim abrirArquivo

'abrirArquivo = Shell("C:\Program Files (x86)\Atwin71\atwin71.exe", vbMaximizedFocus)

Dim ConcedeBPCcomRL, ReturnValue

    'AbrirPrisma = Shell("C:\Program Files (x86)\Atwin71\atwin71.exe", vbMaximizedFocus)
    AppActivate (Shell("C:\Program Files (x86)\Atwin71\atwin71.exe", vbMaximizedFocus)) ' Abrir Prisma
    'Application.SendKeys ("%{TAB}") ' ALT+TAB
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys (Range("A2").Text) 'OL
    Call MyWaitMacro
    Application.SendKeys ("~") 'Tecla Enter
    Call MyWaitMacro
    Application.SendKeys (Range("B2").Text) 'Matrícula
    Application.SendKeys ("~") 'Tecla Enter
    Call MyWaitMacro
    Application.SendKeys (Range("C2").Text) 'Senha
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("2") 'DESCER O CURSOR DO TECLADO PARA HABILITAÇÃO
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("~")
    Application.SendKeys ("~")
    Application.SendKeys (Range("J2").Text) 'ESPECIE
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("4") 'MOTIVO JUDICIAL
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys (Range("D2").Text) 'OL
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("99") 'LIBERA TEMPO
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("T") 'TITULAR
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("N") 'NÃO É ACORDO INTERNACIONAL
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys (Range("F2").Text) 'NIT
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call AguardaCinco
    Application.SendKeys ("~")
    Call AguardaCinco
    Application.SendKeys ("S") 'CONSULTAR CNIS
    Call AguardaDez
    Application.SendKeys ("~") 'ENTER
    Call AguardaCinco
    Application.SendKeys ("S") 'CONFIRMAR DADOS DO TITULAR
    Call AguardaDez
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("~") 'Existência de BPC indeferido
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("~") 'CAPA DE PROCESSO
    Call MyWaitMacro
    Application.SendKeys ("N") 'NÃO IMPRIMIR CAPA DE PROCESSO
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("N") 'NÃO IMPRIMIR PROTOCOLO
    Call AguardaCinco
    Application.SendKeys ("~")
    Call AguardaCinco
    Application.SendKeys ("N")
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("1702180") 'APS MANTENEDORA
    Call MyWaitMacro
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("~") 'ENTER PARA INFORMAR QUE NÃO É EX COMBATENTE
    Call MyWaitMacro
    Application.SendKeys ("18") 'INFORMAR A DIB
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys (Range("G2").Text) 'OL
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("19") 'INFORMAR A DIP
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys (Range("H2").Text) 'OL
    Application.SendKeys ("~")
    Call MyWaitMacro
    Application.SendKeys ("~") 'FINALIZAR TELA DO PROTOCOLO
    'Call AguardaCinco
    'Application.SendKeys ("/") 'NÃO É BENEFÍCIO DESDOBRADO
    'Application.SendKeys ("/")
    'Application.SendKeys ("~")
    'Call MyWaitMacro
    'Application.SendKeys ("M") 'MULTIPLA ATIVIDADE
    'Call MyWaitMacro
    'Application.SendKeys ("/") 'VÍNCULOS
    'Call MyWaitMacro
    'Application.SendKeys ("~") 'FIM DA TELA DE VÍNCULOS
    'Call MyWaitMacro
    'Application.SendKeys ("80")
    'Call MyWaitMacro
    'Application.SendKeys ("~")
    'Call MyWaitMacro
    'Application.SendKeys ("P")
    'Call MyWaitMacro
    'Application.SendKeys ("~")
    'Application.SendKeys ("~")
    'Application.SendKeys ("~")
    'Call MyWaitMacro
    'Application.SendKeys ("T")
    'Call MyWaitMacro
    'Application.SendKeys ("~")
    'Call MyWaitMacro
    'Application.SendKeys ("~")
    'Call MyWaitMacro
    'Application.SendKeys ("?")
    'Call MyWaitMacro
    'Application.SendKeys ("~")

End Sub

Sub MyWaitMacro()
newHour = Hour(Now())
newMinute = Minute(Now())
newSecond = Second(Now()) + 2
waitTime = TimeSerial(newHour, newMinute, newSecond)
Application.Wait waitTime
End Sub

Sub AguardaCinco()
newHour = Hour(Now())
newMinute = Minute(Now())
newSecond = Second(Now()) + 5
waitTime = TimeSerial(newHour, newMinute, newSecond)
Application.Wait waitTime
End Sub

Sub AguardaDez()
newHour = Hour(Now())
newMinute = Minute(Now())
newSecond = Second(Now()) + 5
waitTime = TimeSerial(newHour, newMinute, newSecond)
Application.Wait waitTime
End Sub


