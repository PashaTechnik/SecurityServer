//
//  MainViewController.swift
//  SecurityClient
//
//  Created by pasha on 22.11.2021.
//

import UIKit

class MainViewController: UIViewController {

    
    var name: String = "1"
    var phone: Int = 1
    
    @IBOutlet weak var nameLabel: UILabel!
    @IBOutlet weak var phoneLabel: UILabel!
    override func viewDidLoad() {
        super.viewDidLoad()

        nameLabel.text = name
        phoneLabel.text = String(phone)
        
    }

}
