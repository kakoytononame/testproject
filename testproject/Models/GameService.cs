using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using testproject.Entities;

namespace testproject.Models
{
    public class GameService
    {
        private readonly ApplicationContext _context;
        public GameService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetGamesAsync()
        {
            return await _context.Games.ToListAsync();
        }

        public async Task<GameDTO> CreateGameAsinc(int ownerId)
        {
            int[] gameBoard = new int[9];

            Game game = new()
            {
                FirstPlayerid = ownerId,
                Gameboard= JsonSerializer.Serialize(gameBoard),
                GameStatus="Waiting"
            };
            try
            {
                await _context.Games.AddAsync(game);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Не удалось создать игру");
            }
            

            GameDTO gameDTO = new()
            {
                GameStatus = game.GameStatus,
                GameBoardJson = game.Gameboard
            };
            
            
            return gameDTO;
        }

        public async Task<GameDTO> JoinGameAsinc(int secondPlayer,int gameId)
        {
            Game? game = await _context.Games.Where(p=>p.Id==gameId).FirstOrDefaultAsync();

            if(game is null)
            {
                throw new Exception("Игра не найдена");
            }

            game.SecondPlayerid = secondPlayer;
            game.GameStatus = "InProcess";

            await _context.SaveChangesAsync();

            GameDTO gameDTO = new()
            {
                GameStatus = game.GameStatus,
                GameBoardJson = game.Gameboard,
                Id = game.Id
            };
            return gameDTO;
        }

        public async Task<GameDTO> DoStepAsinc(int gameId,int index,int value )
        {
            Game? game = await _context.Games.Where(p => p.Id == gameId).FirstOrDefaultAsync();

            if (game is null|| string.IsNullOrWhiteSpace(game.Gameboard))
            {
                throw new Exception("Игра не найдена");
            }

            int[]? gameBoard = JsonSerializer.Deserialize<int[]>(game.Gameboard);

            if(gameBoard is  null)
            {
                throw new Exception("Ошибка на сервере");
            }

            if (gameBoard[index] != 0)
            {
                throw new Exception("Поле уже заполнено");
            }
            EndGame(game);
            if (game.GameStatus != "End")
            {
                gameBoard[index] = value;

                game.Gameboard = JsonSerializer.Serialize(gameBoard);

                await _context.SaveChangesAsync();
            }
           
            GameDTO gameDTO = new()
            {
                GameStatus = game.GameStatus,
                GameBoardJson = game.Gameboard,
                Id = game.Id
            };

            return gameDTO;
        }

        private void EndGame(Game game)
        {
            int[]? gameBoard = JsonSerializer.Deserialize<int[]>(game.Gameboard);
            int counter = 0;
            int elementscounter = 0;
            foreach(var item in gameBoard)
            {
                if (item != 0)
                {
                    elementscounter++;
                }
                

                counter++;
                if(elementscounter == gameBoard.Length)
                {
                    game.GameStatus = "End";
                }
            }
            
             CheckForWin(game, "Cross", gameBoard);

             if (game.GameStatus == "InProcess")
             {
                    CheckForWin(game, "Zero", gameBoard);
             }
                
            
            
        }
        private void CheckForWin(Game game,string type, int[]gameBoard)
        {
            game.GameStatus = "InProcess";
            
            if (type == "Cross")
            {
                int[,] massiv= new int[3, 3];
                int massivindex = 0;
                
                for(int i = 0; i < 3; i++)
                {
                    for(int j = 0; j < 3; j++)
                    {
                        massiv[i, j] = gameBoard[massivindex];
                        massivindex++;
                    }
                }
                
                int winchecker = 0;
                
                for(int i = 0; i < 3; i++)
                {
                    for( int j = 0;j < 3; j++)
                    {
                        if (massiv[i, j] == 1) 
                        {
                            winchecker++;
                        }
                    }
                    
                    if(winchecker == 3)
                    {
                        game.GameStatus = "Win-Cross";
                    }
                    
                    winchecker = 0;
                }
                
                if(game.GameStatus == "InProcess")
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (massiv[i, j] == 1)
                            {
                                winchecker++;
                            }
                        }
                        
                        if (winchecker == 3)
                        {
                            game.GameStatus = "Win-Cross";
                        }
                        
                        winchecker = 0;
                    }
                }
                
                if (game.GameStatus == "InProcess")
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (massiv[i, i] == 1)
                        {
                            winchecker++;
                        }
                        
                        if (winchecker == 3)
                        {
                            game.GameStatus = "Win-Cross";
                        }
                        
                    }
                    
                    winchecker = 0;
                }
                if (game.GameStatus == "InProcess")
                {
                    for (int i = 2; i >= 0; i--)
                    {
                        for(int j = 0;j < 3; j++)
                        {
                            if (massiv[j, i] == 1)
                            {
                                winchecker++;
                            }
                            
                            if (winchecker == 3)
                            {
                                game.GameStatus = "Win-Cross";
                            }
                            
                        }
                        
                    }
                    
                    
                }
            }
            if (type == "Zero")
            {
                game.GameStatus = "InProcess";
                int[,] massiv = new int[3, 3];
                int massivindex = 0;
                
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        massiv[i, j] = gameBoard[massivindex];
                        massivindex++;
                    }
                }
                
                int winchecker = 0;
                
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (massiv[i, j] == 2)
                        {
                            winchecker++;
                        }
                    }
                    
                    if (winchecker == 3)
                    {
                        game.GameStatus = "Win-Zero";
                    }
                    
                    winchecker = 0;
                }
                
                if (game.GameStatus == "InProcess")
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (massiv[i, j] == 2)
                            {
                                winchecker++;
                            }
                        }
                        
                        if (winchecker == 3)
                        {
                            game.GameStatus = "Win-Zero";
                        }
                        
                        winchecker = 0;
                    }
                }
                if (game.GameStatus == "InProcess")
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (massiv[i, i] == 2)
                        {
                            winchecker++;
                        }
                        
                        if (winchecker == 3)
                        {
                            game.GameStatus = "Win-Zero";
                        }

                    }
                    
                    winchecker = 0;
                }
                if (game.GameStatus == "InProcess")
                {
                    for (int i = 2; i >= 0; i--)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (massiv[j, i] == 2)
                            {
                                winchecker++;
                            }
                            
                            if (winchecker == 3)
                            {
                                game.GameStatus = "Win-Zero";
                            }

                        }

                    }
                
                }
            }
        }
    }
}
