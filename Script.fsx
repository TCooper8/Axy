#load "src/Try.fs"
#load "src/Utils.fs"
#load "src/Async.fs"
#load "src/Actor.fs"

open System

open Axy

module ActorTest =
  let actor: int Actor.ActorEvent Actor = Actor.actorOf {
    initialState = 5
    onFailed = fun state e ->
      sprintf "Error: %A" e |> ignore
      Actor.Stopped
    receive = fun x -> function
      | Actor.Notify (y) ->
        Actor.Running y
  }

  for i in 0 .. 1000000 do
    actor.Post (Actor.Notify i)
    printfn "Posted: %A" i