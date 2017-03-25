namespace Axy

module Option =
  let inline getOrElse compensation = function
    | None -> compensation ()
    | Some value -> value

  let inline tap action = function
    | None -> None
    | Some value ->
      action value
      Some value
