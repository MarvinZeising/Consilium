namespace Consilium

module Validation =

    open CommonTypes
    open CommonLibrary

    let private addSuccess r1 r2 = r1 // return first

    let private addFailure s1 s2 = s1 @ s2  // concatf

    let (&&&) v1 v2 = 
        plus addSuccess addFailure v1 v2
 
    let combineResults v1 v2 = 
        map2 addSuccess addFailure v1 v2
