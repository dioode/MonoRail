﻿//  Copyright 2004-2011 Castle Project - http://www.castleproject.org/
//  Hamilton Verissimo de Oliveira and individual contributors as indicated. 
//  See the committers.txt/contributors.txt in the distribution for a 
//  full listing of individual contributors.
// 
//  This is free software; you can redistribute it and/or modify it
//  under the terms of the GNU Lesser General Public License as
//  published by the Free Software Foundation; either version 3 of
//  the License, or (at your option) any later version.
// 
//  You should have received a copy of the GNU Lesser General Public
//  License along with this software; if not, write to the Free
//  Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
//  02110-1301 USA, or see the FSF site: http://www.fsf.org.

[<AutoOpen>]
module RefHelpers
    open System
    open System.Reflection
    open System.Linq.Expressions

    let read_att_filter<'a when 'a : null> (prov:#ICustomAttributeProvider) (filter:'a -> bool) : 'a = 
        let attrs = prov.GetCustomAttributes(typeof<'a>, true)
        if (attrs.Length = 0) then
            null
        else 
            let filtered = attrs |> Array.toSeq |> Seq.cast<'a> |> Seq.filter filter
            if Seq.length filtered = 1 then
                Seq.head filtered
            else
                failwithf "Expected a single %s, but found many in provider %O" (typeof<'a>.Name) (prov.GetType())

    let read_att<'a when 'a : null> (prov:#ICustomAttributeProvider) : 'a = 
        let attrs = prov.GetCustomAttributes(typeof<'a>, true)
        if (attrs.Length = 0) then
            null
        elif (attrs.Length = 1) then
            attrs.[0] :?> 'a
        else
            // failwithf "Expected a single, but found many in provider %O" (prov.GetType())
            failwithf "Expected a single %s, but found many in provider %O" (typeof<'a>.Name) (prov.GetType())
            null

    let propinfo_from_exp (propertyAccess:Expression<Func<'a, obj>>) : PropertyInfo = 
        if propertyAccess.NodeType <> ExpressionType.Lambda then
            raise (ArgumentException ((sprintf "" ), "propertyAccess"))
        else
            let memberAccess = propertyAccess.Body :?> MemberExpression
            memberAccess.Member :?> PropertyInfo
