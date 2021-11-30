//
//  ViewController.swift
//  SecurityClient
//
//  Created by pasha on 22.11.2021.
//

import UIKit

class RegistrationViewController: UIViewController {

    @IBOutlet weak var usernameTextField: UITextField!
    @IBOutlet weak var phoneTextField: UITextField!
    @IBOutlet weak var passwordTextField: UITextField!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        NetworkManager.addUser(User(Name: "Vasya", Phone: 12954374738, Password: "123"))
        
    }
    
    @IBAction func registerUser(_ sender: Any) {
        let newUser = User(Name: usernameTextField.text!, Phone: Int(phoneTextField.text!)!, Password: passwordTextField.text!)
        NetworkManager.addUser(newUser)
    }
    

}

