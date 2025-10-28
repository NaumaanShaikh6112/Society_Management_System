<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Society_Management_System.Account.Register" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Register - Society Management System</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap" rel="stylesheet" />
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Poppins', sans-serif;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 2rem;
            position: relative;
            overflow-x: hidden;
        }

            body::before {
                content: '';
                position: absolute;
                width: 200%;
                height: 200%;
                top: -50%;
                left: -50%;
                background: radial-gradient(circle, rgba(255,255,255,0.1) 1px, transparent 1px);
                background-size: 50px 50px;
                animation: moveBackground 20s linear infinite;
                z-index: 0;
            }

        @keyframes moveBackground {
            0% {
                transform: translate(0, 0);
            }

            100% {
                transform: translate(50px, 50px);
            }
        }

        .register-container {
            position: relative;
            z-index: 1;
            width: 100%;
            max-width: 600px;
        }

        .glass-card {
            background: rgba(255, 255, 255, 0.15);
            backdrop-filter: blur(20px);
            -webkit-backdrop-filter: blur(20px);
            border-radius: 25px;
            border: 1px solid rgba(255, 255, 255, 0.3);
            padding: 3rem;
            box-shadow: 0 15px 45px rgba(0, 0, 0, 0.2);
            animation: slideUp 0.8s ease;
        }

        @keyframes slideUp {
            from {
                opacity: 0;
                transform: translateY(30px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .register-header {
            text-align: center;
            margin-bottom: 2rem;
        }

        .register-icon {
            width: 80px;
            height: 80px;
            background: rgba(255, 255, 255, 0.2);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 1.5rem;
            font-size: 2rem;
            color: #fff;
        }

        .register-title {
            color: #fff;
            font-weight: 700;
            font-size: 2rem;
            margin-bottom: 0.5rem;
            text-shadow: 2px 2px 4px rgba(0,0,0,0.2);
        }

        .register-subtitle {
            color: rgba(255, 255, 255, 0.9);
            font-size: 1rem;
        }

        .form-group {
            margin-bottom: 1.3rem;
            position: relative;
        }

        .form-label {
            color: #fff;
            font-weight: 500;
            margin-bottom: 0.5rem;
            display: block;
            font-size: 0.95rem;
        }

        .input-wrapper {
            position: relative;
        }

        .input-icon {
            position: absolute;
            left: 1rem;
            top: 50%;
            transform: translateY(-50%);
            color: rgba(255, 255, 255, 0.7);
            font-size: 1rem;
            z-index: 1;
        }

        .form-control, .form-select {
            background: rgba(255, 255, 255, 0.1);
            border: 1px solid rgba(255, 255, 255, 0.3);
            border-radius: 50px;
            padding: 0.8rem 1rem 0.8rem 3rem;
            color: #fff;
            font-size: 0.95rem;
            transition: all 0.3s ease;
        }

        .form-select {
            cursor: pointer;
        }

            .form-select option {
                background: #764ba2;
                color: #fff;
            }

            .form-control:focus, .form-select:focus {
                background: rgba(255, 255, 255, 0.15);
                border-color: rgba(255, 255, 255, 0.5);
                box-shadow: 0 0 20px rgba(255, 255, 255, 0.2);
                color: #fff;
                outline: none;
            }

        .form-control::placeholder {
            color: rgba(255, 255, 255, 0.6);
        }

        .btn-register {
            width: 100%;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border: none;
            border-radius: 50px;
            padding: 1rem;
            font-size: 1.1rem;
            font-weight: 600;
            color: #fff;
            transition: all 0.3s ease;
            box-shadow: 0 5px 20px rgba(0,0,0,0.2);
            margin-top: 1rem;
        }

            .btn-register:hover {
                transform: translateY(-2px);
                box-shadow: 0 8px 25px rgba(0,0,0,0.3);
            }

        .error-message, .success-message {
            border-radius: 15px;
            padding: 1rem;
            margin-bottom: 1.5rem;
            text-align: center;
        }

        .error-message {
            background: rgba(220, 53, 69, 0.2);
            border: 1px solid rgba(220, 53, 69, 0.5);
            color: #fff;
        }

        .success-message {
            background: rgba(40, 167, 69, 0.2);
            border: 1px solid rgba(40, 167, 69, 0.5);
            color: #fff;
        }

        .form-footer {
            text-align: center;
            margin-top: 1.5rem;
            color: rgba(255, 255, 255, 0.9);
        }

            .form-footer a {
                color: #fff;
                font-weight: 600;
                text-decoration: none;
            }

                .form-footer a:hover {
                    text-decoration: underline;
                }

        .back-home {
            position: absolute;
            top: 2rem;
            left: 2rem;
            z-index: 10;
        }

        .btn-back {
            background: rgba(255, 255, 255, 0.2);
            border: 1px solid rgba(255, 255, 255, 0.3);
            border-radius: 50px;
            padding: 0.7rem 1.5rem;
            color: #fff;
            text-decoration: none;
            transition: all 0.3s ease;
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
        }

            .btn-back:hover {
                background: rgba(255, 255, 255, 0.3);
                transform: translateX(-5px);
            }

        .row {
            margin-left: -0.5rem;
            margin-right: -0.5rem;
        }

            .row > div {
                padding-left: 0.5rem;
                padding-right: 0.5rem;
            }

        .validation-message {
            color: #fff;
            font-size: 0.85rem;
            margin-top: 0.3rem;
            display: block;
        }

        @media (max-width: 576px) {
            body {
                padding: 1rem;
            }

            .glass-card {
                padding: 2rem 1.5rem;
            }

            .register-title {
                font-size: 1.5rem;
            }

            .back-home {
                top: 1rem;
                left: 1rem;
            }
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="back-home">
            <asp:HyperLink ID="lnkBackHome" runat="server" NavigateUrl="~/Default.aspx" CssClass="btn-back">
            <i class="fas fa-arrow-left"></i> Back to Home
            </asp:HyperLink>
        </div>

        <div class="register-container">
            <div class="glass-card">
                <div class="register-header">
                    <div class="register-icon">
                        <i class="fas fa-user-plus"></i>
                    </div>
                    <h1 class="register-title">Create Account</h1>
                    <p class="register-subtitle">Join our community today</p>
                </div>

                <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="error-message">
                    <i class="fas fa-exclamation-circle"></i>
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                </asp:Panel>

                <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="success-message">
                    <i class="fas fa-check-circle"></i>
                    <asp:Label ID="lblSuccess" runat="server"></asp:Label>
                </asp:Panel>

                <asp:ValidationSummary ID="valSummary" runat="server" CssClass="validation-message" ValidationGroup="RegisterVG" DisplayMode="BulletList" />

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="form-label" for="txtFullName">Full Name</label>
                            <div class="input-wrapper">
                                <i class="fas fa-user input-icon"></i>
                                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" Placeholder="John Doe" ValidationGroup="RegisterVG" />
                            </div>
                            <asp:RequiredFieldValidator ID="reqFullName" runat="server" ControlToValidate="txtFullName"
                                ErrorMessage="Full name is required." CssClass="validation-message" ValidationGroup="RegisterVG" Display="Dynamic" />
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="form-label" for="txtEmail">Email</label>
                            <div class="input-wrapper">
                                <i class="fas fa-envelope input-icon"></i>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="you@example.com" ValidationGroup="RegisterVG" />
                            </div>
                            <asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="Email is required." CssClass="validation-message" ValidationGroup="RegisterVG" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="Enter a valid email address." CssClass="validation-message" ValidationGroup="RegisterVG"
                                ValidationExpression="^[\w\.\-]+@([\w\-]+\.)+[a-zA-Z]{2,}$" Display="Dynamic" />
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="form-label" for="txtMobile">Mobile Number</label>
                            <div class="input-wrapper">
                                <i class="fas fa-phone input-icon"></i>
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" Placeholder="+1234567890" ValidationGroup="RegisterVG" />
                            </div>
                            <asp:RequiredFieldValidator ID="reqMobile" runat="server" ControlToValidate="txtMobile"
                                ErrorMessage="Mobile number is required." CssClass="validation-message" ValidationGroup="RegisterVG" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="txtMobile"
                                ErrorMessage="Enter a valid mobile number." CssClass="validation-message" ValidationGroup="RegisterVG"
                                ValidationExpression="^\+?[0-9]{7,15}$" Display="Dynamic" />
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="form-label" for="txtFlatNo">Flat / Unit No.</label>
                            <div class="input-wrapper">
                                <i class="fas fa-door-closed input-icon"></i>
                                <asp:TextBox ID="txtFlatNo" runat="server" CssClass="form-control" Placeholder="A-101" ValidationGroup="RegisterVG" />
                            </div>
                            <asp:RequiredFieldValidator ID="reqFlatNo" runat="server" ControlToValidate="txtFlatNo"
                                ErrorMessage="Flat/Unit number is required." CssClass="validation-message" ValidationGroup="RegisterVG" Display="Dynamic" />
                        </div>
                    </div>
                </div>

                <asp:UpdatePanel ID="updSocietyBuilding" runat="server">
                    <ContentTemplate>
                        <!-- Society Dropdown -->
                        <div class="form-group">
                            <label class="form-label" for="ddlSociety">Select Society</label>
                            <asp:DropDownList ID="ddlSociety" runat="server"
                                CssClass="form-control"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlSociety_SelectedIndexChanged"
                                ValidationGroup="RegisterVG" />
                            <asp:RequiredFieldValidator ID="reqSociety" runat="server" ControlToValidate="ddlSociety"
                                ErrorMessage="Society is required." CssClass="validation-message" ValidationGroup="RegisterVG" Display="Dynamic" />
                        </div>
                        <!-- Building Dropdown -->
                        <div class="form-group">
                            <label class="form-label" for="ddlBuilding">Select Building</label>
                            <asp:DropDownList ID="ddlBuilding" runat="server" CssClass="form-control" ValidationGroup="RegisterVG" />
                            <asp:RequiredFieldValidator ID="reqBuilding" runat="server" ControlToValidate="ddlBuilding"
                                ErrorMessage="Building is required." CssClass="validation-message" ValidationGroup="RegisterVG" Display="Dynamic" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="form-group">
                    <label class="form-label" for="ddlOccupancyType">Occupancy Type</label>
                    <asp:DropDownList ID="ddlOccupancyType" runat="server" CssClass="form-control" ValidationGroup="RegisterVG">
                        <asp:ListItem Text="Select" Value="" />
                        <asp:ListItem Text="Owner" Value="Owner" />
                        <asp:ListItem Text="Tenant" Value="Tenant" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqOccupancyType" runat="server" ControlToValidate="ddlOccupancyType"
                        ErrorMessage="Occupancy type is required." CssClass="validation-message" ValidationGroup="RegisterVG" Display="Dynamic" />
                </div>

                <asp:HiddenField ID="hdnRoleId" runat="server" Value="2" />

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="form-label" for="txtPassword">Password</label>
                            <div class="input-wrapper">
                                <i class="fas fa-lock input-icon"></i>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="••••••••" ValidationGroup="RegisterVG" />
                            </div>
                            <asp:RequiredFieldValidator ID="reqPassword" runat="server" ControlToValidate="txtPassword"
                                ErrorMessage="Password is required." CssClass="validation-message" ValidationGroup="RegisterVG" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword"
                                ErrorMessage="Password must be at least 6 characters." CssClass="validation-message" ValidationGroup="RegisterVG"
                                ValidationExpression="^.{6,}$" Display="Dynamic" />
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="form-label" for="txtConfirmPassword">Confirm Password</label>
                            <div class="input-wrapper">
                                <i class="fas fa-lock input-icon"></i>
                                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="••••••••" ValidationGroup="RegisterVG" />
                            </div>
                            <asp:RequiredFieldValidator ID="reqConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
                                ErrorMessage="Please confirm your password." CssClass="validation-message" ValidationGroup="RegisterVG" Display="Dynamic" />
                            <asp:CompareValidator ID="cmpPasswords" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword"
                                ErrorMessage="Passwords do not match." CssClass="validation-message" ValidationGroup="RegisterVG" Display="Dynamic" />
                        </div>
                    </div>
                </div>

                <asp:Button ID="btnRegister" runat="server" CssClass="btn-register" Text="Create Account" OnClick="btnRegister_Click" ValidationGroup="RegisterVG" />

                <div class="form-footer">
                    <p>Already have an account?
                        <asp:HyperLink ID="lnkLogin" runat="server" NavigateUrl="~/Account/Login.aspx">Sign in</asp:HyperLink></p>
                </div>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
