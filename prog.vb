imports System.Threading
module exemplo
#region "VARIAVEIS"
	dim threadDisparo as Thread
	dim saida as boolean = false
	dim pontos as integer = 0
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	dim timerProj as System.Timers.Timer
	dim timerInimigo as System.Timers.Timer
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	dim mapCol as integer = 40
	dim mapLin as integer = 20
	dim mapChar as char = "#"
	dim mapBlank as char = " "
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	dim pChar as char = "A"
	dim pPosInit as integer = 20
	dim pPos as integer = 0
	dim pPosOld as integer = 0
	dim pLin as integer = 17
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	dim projChar as char = "!"
	dim projPos as integer
	dim projPosOld as integer
	dim projCol as integer
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	structure tipoInimigo
		dim Pos as integer
		dim PosOld as integer
		dim Col as integer
		dim Atingido as boolean
		dim Caracter as char
	end structure
	dim inimigo(10) as tipoInimigo
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	dim cKey as ConsoleKeyInfo
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub desenharPontos()
		console.setcursorposition(mapCol + 2,1)
		console.write("Pontos: " & pontos)
	end sub
#end region
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub desenharMapa()
		dim cont,contb as integer
		console.clear()
		for cont = 0 to mapCol
			for contb = 0 to mapLin
				if cont > 0 and cont < mapCol and contb > 0 and contb < mapLIn then
					console.setcursorposition(cont,contb)
					console.write(mapBlank)
				else
					console.setcursorposition(cont,contb)
					console.write(mapChar)
				end if
			next
		next
	end sub
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub inicializarPlayer()
		console.setcursorposition(pPosInit,pLin)
		console.write(pChar)
		pPos = pPosInit
	end sub
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub pegarTecla()
		cKey = console.readkey
		if cKey.Key <> ConsoleKey.Spacebar then
			if cKey.Key = ConsoleKey.LeftArrow then
				if pPos > 1 then
					pPosOld = pPos
					pPos -= 1
				end if
			end if
			if cKey.Key = ConsoleKey.RightArrow then
				if pPos < mapCol - 1 
					pPosOld = pPos
					pPos += 1
				end if
			end if
			console.setcursorposition(pPosOld,pLin)
			console.write(mapBlank)
			console.setcursorposition(pPos,pLin)
			console.write(pChar)
		else
			dispararProjetil()
		end if		
	end sub
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub dispararProjetil()
		projPos = pLin - 1
		projCol = pPos
		console.setcursorposition(projCol,projPos)
		console.write(projChar)
		timerProj.start
	end sub
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub moverProjetil()
		if projPos > 1 then
			projPosOld = projPos
			projPos -= 1
			console.setcursorposition(projCol,projPosOld)
			console.write(mapBlank)
			console.setcursorposition(projCol,projPos)
			console.write(projChar)
			dim cont as integer
			for cont = 1 to 10
				if projPos = inimigo(cont).Pos and projCol = inimigo(cont).Col then
					inimigo(cont).Atingido = true
					inimigo(cont).Pos = 0
					inimigo(cont).Col = 0
					pontos += 1
					desenharPontos()
				end if
			next
		else
			console.setcursorposition(projCol,projPos)
			console.write(mapBlank)
			timerProj.stop
		end if
	end sub
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub inicializarInimigo()
		Randomize()
		dim cont as integer
		dim randN as integer
		dim numPos(10) as integer
		dim matchPos as boolean = false
		dim tmpCont as integer = 1
		for cont = 1 to 10
			inimigo(cont) = new tipoInimigo
			randN = (36 * rnd() + 2)
			numPos(cont) = randN
			inimigo(cont).Col = numPos(cont)
			inimigo(cont).Pos = 2
			inimigo(cont).Caracter = "W"
		next
		for cont = 1 to 10
			console.setcursorposition(inimigo(cont).Col,inimigo(cont).Pos)
			console.write(inimigo(cont).Caracter)
		next
		timerInimigo.start()
	end sub
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub moverInimigo(byval numInimigo as integer)
		if inimigo(numInimigo).Atingido = false then
			inimigo(numInimigo).PosOld = inimigo(numInimigo).Pos
			inimigo(numInimigo).Pos += 1
			console.setcursorposition(inimigo(numInimigo).col,inimigo(numInimigo).pos)
			console.write(inimigo(numInimigo).caracter)
			console.setcursorposition(inimigo(numInimigo).col,inimigo(numInimigo).PosOld)
			console.write(mapBlank)
		end if
	end sub
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub loopMoverInimigo()
		dim cont as integer = 1
		for cont = 1 to 10
			moverInimigo(cont)
		next
	end sub
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub main()		
		timerProj = new System.Timers.Timer
		timerProj.interval = 50
		timerProj.enabled = true
		AddHandler timerProj.Elapsed, AddressOf moverProjetil
		timerInimigo = new System.Timers.Timer
		timerInimigo.interval = 1500
		timerInimigo.enabled = true
		AddHandler timerInimigo.Elapsed, AddressOf loopMoverInimigo
		desenharMapa()
		inicializarPlayer()
		inicializarInimigo()
		while(not saida)
			pegarTecla()
				'CONSERTAR CARACTERES VAZIOS NA BORDA DO MAPA
				console.setcursorposition(0,0)
				console.write(mapChar)
				console.setcursorposition(1,0)
				console.write(mapChar)
				'''''''''''''''''''''''''''''''''''''''''''''
			
			console.setcursorposition(0,mapLin + 1)			
		end while
	end sub
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
end module