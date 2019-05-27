module TestLibrary

open Shouldly
open Consilium
open CommonTypes

let shouldSucceedWith result (value: Result<'a, Error list>) =
    match value with
    | Ok x -> x.ShouldBe result
    | Error _ -> raise (System.Exception "Should succeed")

let shouldContainError (error: Error) (value: Result<'a, Error list>) =
    match value with
    | Ok _ -> raise (System.Exception "Should throw error")
    | Error x ->
        x.ShouldContain error
        true
