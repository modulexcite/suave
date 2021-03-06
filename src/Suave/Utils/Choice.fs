﻿namespace Suave.Utils

open System

module Choice =

  let mreturn x = Choice1Of2 x

  let map f = function
    | Choice1Of2 v   -> Choice1Of2 (f v)
    | Choice2Of2 err -> Choice2Of2 err

  let mapError f = function
    | Choice1Of2 v   -> Choice1Of2 v
    | Choice2Of2 err -> Choice2Of2 (f err)

  let bind (f : 'a -> Choice<'b, 'c>) (v : Choice<'a, 'c>) =
    match v with
    | Choice1Of2 v -> f v
    | Choice2Of2 c -> Choice2Of2 c

  let ofOption onMissing = function
    | Some x -> Choice1Of2 x
    | None   -> Choice2Of2 onMissing

  [<Obsolete "use Choice.ofOption instead - for consistency w/ F# naming stds">]
  let fromOption onMissing = ofOption onMissing

  let orDefault onMissing = function
    | Choice1Of2 x -> x
    | Choice2Of2 _ -> onMissing

  let iter f = function
    | Choice1Of2 x -> f x
    | Choice2Of2 _ -> ()

module ChoiceOperators =

  let (|@) opt errorMsg =
    match opt with
    | Some x -> Choice1Of2 x
    | None -> Choice2Of2 errorMsg

  let (||@) c errorMsg =
    match c with
    | Choice1Of2 x -> Choice1Of2 x
    | Choice2Of2 y -> Choice2Of2 errorMsg