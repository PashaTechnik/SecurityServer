//
//  User.swift
//  SecurityClient
//
//  Created by pasha on 22.11.2021.
//

import Foundation


struct User: Codable {
    var id: Int?
    var name: String
    var phone: Int
    var password: String
}
