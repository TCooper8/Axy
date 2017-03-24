namespace Axiom

type 'a Try =
  | Success of 'a
  | Failure of exn

module Try =
  let success = Success
  let failure = Failure

  let inline internal _try action recovery =
    try action ()
    with e -> recovery e

  let inline map mapping = function
    | Success value ->
      _try
      <| fun () -> value |> mapping |> Success
      <| Failure
    | any -> any

  let inline bind binding = function
    | Success value ->
      _try
      <| fun () -> value |> binding
      <| Failure
    | any -> any

  let inline tap action = function
    | Success value ->
      action value
      Success value
    | any -> any

  let inline recover recovery = function
    | Failure e -> recovery e
    | Success value -> Success value
