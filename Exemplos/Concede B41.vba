' Concede B41
' Riscos: 
'    Se tiver caractere invalido no cadastro do CNIS
'    Não pode haver vinculos no NIT.

Sub abrir_Prisma()

'Dim abrirArquivo

'abrirArquivo = Shell("C:\Program Files (x86)\Atwin71\atwin71.exe", vbMaximizedFocus)

Dim AbrirPrisma, ReturnValue

'AbrirPrisma = Shell("C:\Program Files (x86)\Atwin71\atwin71.exe", vbMaximizedFocus)
AppActivate (Shell("C:\Program Files (x86)\Atwin71\atwin71.exe", vbMaximizedFocus)) ' Abrir Prisma
'Application.SendKeys ("%{TAB}") ' ALT+TAB
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("17021200") 'OL
Call MyWaitMacro
Application.SendKeys ("~") 'Tecla Enter
Call MyWaitMacro
Application.SendKeys ("Matr�cula_Pessoal") 'Matricula
Application.SendKeys ("~") 'Tecla Enter
Call MyWaitMacro
Application.SendKeys ("Senha_Pessoal") 'SENHA
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
Application.SendKeys ("2") 'DESCER O CURSOR DO TECLADO PARA HABILITA��O
Call MyWaitMacro
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("~")
Application.SendKeys ("~")
Application.SendKeys ("41") 'ESPECIE
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("4") 'MOTIVO JUDICIAL
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("00013172620188190025") 'N�MERO DO PJ
Call MyWaitMacro
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("11") 'LIBERA TEMPO
Call MyWaitMacro
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("T") 'TITULAR
Call MyWaitMacro
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("N") 'N�O � ACORDO INTERNACIONAL
Call MyWaitMacro
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("8") 'RURAL
Call MyWaitMacro
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("7") 'ESPECIAL
Call MyWaitMacro
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("16867153772") 'NIT
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
Application.SendKeys ("~") 'AGENDAMENTO N�O ENCONTRADO
Call MyWaitMacro
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("~") 'CAPA DE PROCESSO
Call MyWaitMacro
Application.SendKeys ("N") 'N�O IMPRIMIR CAPA DE PROCESSO
Call MyWaitMacro
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("N") 'N�O IMPRIMIR PROTOCOLO
Call AguardaCinco
Application.SendKeys ("~")
Call AguardaCinco
Application.SendKeys ("N") 'APS MANTENEDORA
Call MyWaitMacro
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("17021110")
Call MyWaitMacro
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("~") 'ENTER PARA INFORMAR QUE N�O � EX COMBATENTE
Call MyWaitMacro
Application.SendKeys ("18") 'INFORMAR A DIB
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("05102017") 'DIB
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("19") 'INFORMAR A DIP
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("01042019") 'DIP
Application.SendKeys ("~")
Call MyWaitMacro
Application.SendKeys ("~") 'FINALIZAR TELA DO PROTOCOLO
'Call AguardaCinco
'Application.SendKeys ("/") 'N�O � BENEF�CIO DESDOBRADO
'Application.SendKeys ("/")
'Application.SendKeys ("~")
'Call MyWaitMacro
'Application.SendKeys ("M") 'MULTIPLA ATIVIDADE
'Call MyWaitMacro
'Application.SendKeys ("/") 'V�NCULOS
'Call MyWaitMacro
'Application.SendKeys ("~") 'FIM DA TELA DE V�NCULOS
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
