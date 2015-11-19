imports System.Threading
module spaceInvader
#region "Global Variables"
	dim threadShot as Thread
	dim exit as boolean = false
	dim score as integer = 0
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	dim timerBullet as System.Timers.Timer
	dim timerEnemy as System.Timers.Timer
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
	dim bulletChar as char = "!"
	dim bulletPos as integer
	dim bulletPosOld as integer
	dim bulletCol as integer
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	structure typeEnemy
		dim Pos as integer
		dim PosOld as integer
		dim Col as integer
		dim GotHit as boolean
		dim EnemyChar as char
	end structure
	dim pEnemy(10) as typeEnemy
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	dim cKey as ConsoleKeyInfo
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub drawScore()
		console.setcursorposition(mapCol + 2,1)
		console.write("Score: " & score)
	end sub
#end region
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub drawMap()
		dim cont,contb as integer
		console.clear()
		for cont = 0 to mapCol
			for contb = 0 to mapLin
				if cont > 0 and cont < mapCol and contb > 0 and contb < mapLin then
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
	sub getKey()
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
			fireBullet()
		end if		
	end sub
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub fireBullet()
		bulletPos = pLin - 1
		bulletCol = pPos
		console.setcursorposition(bulletCol,bulletPos)
		console.write(bulletChar)
		timerBullet.start
	end sub
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub moveBullet()
		if bulletPos > 1 then
			bulletPosOld = bulletPos
			bulletPos -= 1
			console.setcursorposition(bulletCol,bulletPosOld)
			console.write(mapBlank)
			console.setcursorposition(bulletCol,bulletPos)
			console.write(bulletChar)
			dim cont as integer
			for cont = 1 to 10
				if bulletPos = pEnemy(cont).Pos and bulletCol = pEnemy(cont).Col then
					pEnemy(cont).GotHit = true
					pEnemy(cont).Pos = 0
					pEnemy(cont).Col = 0
					score += 1
					drawScore()
				end if
			next
		else
			console.setcursorposition(bulletCol,bulletPos)
			console.write(mapBlank)
			timerBullet.stop
		end if
	end sub
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub inicializeEnemy()
		Randomize()
		dim cont as integer
		dim randN as integer
		dim numPos(10) as integer
		dim matchPos as boolean = false
		dim tmpCont as integer = 1
		for cont = 1 to 10
			pEnemy(cont) = new typeEnemy
			randN = (36 * rnd() + 2)
			numPos(cont) = randN
			pEnemy(cont).Col = numPos(cont)
			pEnemy(cont).Pos = 2
			pEnemy(cont).EnemyChar = "W"
		next
		for cont = 1 to 10
			console.setcursorposition(pEnemy(cont).Col,pEnemy(cont).Pos)
			console.write(pEnemy(cont).EnemyChar)
		next
		timerEnemy.start()
	end sub
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub moveEnemy(byval enemyIndex as integer)
		if pEnemy(enemyIndex).GotHit = false then
			pEnemy(enemyIndex).PosOld = pEnemy(enemyIndex).Pos
			pEnemy(enemyIndex).Pos += 1
			console.setcursorposition(pEnemy(enemyIndex).col,pEnemy(enemyIndex).pos)
			console.write(pEnemy(enemyIndex).EnemyChar)
			console.setcursorposition(pEnemy(enemyIndex).col,pEnemy(enemyIndex).PosOld)
			console.write(mapBlank)
		end if
	end sub
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub loopMoveEnemy()
		dim cont as integer = 1
		for cont = 1 to 10
			moveEnemy(cont)
		next
	end sub
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''	
	sub main()		
		timerBullet = new System.Timers.Timer
		timerBullet.interval = 50
		timerBullet.enabled = true
		AddHandler timerBullet.Elapsed, AddressOf moveBullet
		timerEnemy = new System.Timers.Timer
		timerEnemy.interval = 1500
		timerEnemy.enabled = true
		AddHandler timerEnemy.Elapsed, AddressOf loopMoveEnemy
		drawMap()
		inicializarPlayer()
		inicializeEnemy()
		while(not exit)
			getKey()
				'Fix some empty characters at the border of the map... At least a try...
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