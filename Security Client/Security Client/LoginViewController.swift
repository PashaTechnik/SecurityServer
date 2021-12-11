//
//  LoginViewController.swift
//  SecurityClient
//
//  Created by pasha on 22.11.2021.
//

import UIKit
import CryptoSwift

class LoginViewController: UIViewController {

    var currentUser: User!
    let group = DispatchGroup()
    
    @IBOutlet weak var errorLabel: UILabel!
    @IBOutlet weak var nameTexxtField: UITextField!
    @IBOutlet weak var passwordTextField: UITextField!
    override func viewDidLoad() {
        super.viewDidLoad()

        nameTexxtField.delegate = self
        passwordTextField.delegate = self
        
    }
    @IBAction func logIn(_ sender: Any) {
        group.enter()
        let hashed = passwordTextField.text!.sha3(.sha512)
        let user = User(id: nil, name: nameTexxtField.text!, phone: 0, password: hashed)
        errorLabel.isHidden = false
        NetworkManager.verifyPassword(user, errorLabel) { [self] user in
            currentUser = user
            group.leave()
        }
        group.wait()
        if currentUser.name != "" {
            self.performSegue(withIdentifier: "goToMain", sender: self)
        }
//        DispatchQueue.main.asyncAfter(deadline: .now() + 2) {
//            self.performSegue(withIdentifier: "goToMain", sender: self)
//        }
        
        
    }
    
    
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        if let vc = segue.destination as? MainViewController {
            vc.name = currentUser.name
            vc.phone = currentUser.phone
        }
    }
    
    @IBAction func goToRegistration(_ sender: Any) {
    }
}

extension LoginViewController: UITextFieldDelegate {
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        textField.resignFirstResponder()
        return true
    }
}

