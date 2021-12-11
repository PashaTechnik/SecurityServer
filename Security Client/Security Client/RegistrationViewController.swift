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
    
    @IBOutlet weak var errorLabel: UILabel!
    override func viewDidLoad() {
        super.viewDidLoad()
        usernameTextField.delegate = self
        phoneTextField.delegate = self
        passwordTextField.delegate = self
        
        passwordTextField.textContentType = .oneTimeCode
        
        
        //NetworkManager.addUser(User(name: "Vasya", phone: 12954374738, password: "123"))
        
    }
    
    @IBAction func registerUser(_ sender: Any) {
        
        if isValidPassword(passwordTextField.text!) && !checkCommon(passwordTextField.text!) {
            let hashed = passwordTextField.text!.sha3(.sha512)
            let newUser = User(name: usernameTextField.text!, phone: Int(phoneTextField.text!)!, password: hashed)
            NetworkManager.addUser(newUser)
            performSegue(withIdentifier: "goToLogin", sender: self)
        } else {
            
            if !isValidPassword(passwordTextField.text!) {
                errorLabel.isHidden = false
                errorLabel.text = "Password must contain minimum 8 characters, at least 1 Uppercase Alphabet, 1 Lowercase Alphabet and 1 Number!"
            }
            
            if checkCommon(passwordTextField.text!) {
                errorLabel.isHidden = false
                errorLabel.text = "Not strong Password!"
            }
            
            
        }
        
    }
    
    func isValidPassword(_ mypassword : String) -> Bool {

        let passwordreg =  ("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$")
        let passwordtesting = NSPredicate(format: "SELF MATCHES %@", passwordreg)
        return passwordtesting.evaluate(with: mypassword)

    }
    
    func checkCommon(_ mypassword: String) -> Bool {
        if let filepath = Bundle.main.path(forResource: "10k-most-common", ofType: "txt") {
            do {
                let contents = try String(contentsOfFile: filepath)
                if contents.split(separator: "\n").map({ String($0)}).contains(mypassword) {
                    return true
                }
            } catch {
                print("Could not load file")
            }
        }
        return false
    }

    

}

extension RegistrationViewController: UITextFieldDelegate {
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        textField.resignFirstResponder()
        return true
    }
}

extension UIViewController {
    open override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        self.view.endEditing(true)
    }
}

