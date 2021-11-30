//
//  NetworkManager.swift
//  SecurityClient
//
//  Created by pasha on 22.11.2021.
//

import Foundation


class NetworkManager {
    
    static func addUser(_ user: User){
        
        let url = URL(string: "http://localhost:5000/api/user")!

        let encoder = JSONEncoder()

        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.httpBody = try? encoder.encode(user)

        URLSession.shared.dataTask(with: request) { data, response, error in
            if let data = data {
                let decoder = JSONDecoder()
                
                if error != nil {
                    print(error?.localizedDescription)
                }
                
                print(response)

                if let item = try? decoder.decode(User.self, from: data) {
                    print(item.Name)
                } else {
                    print("Bad JSON received back.")
                }
            }
        }.resume()
    }
}
