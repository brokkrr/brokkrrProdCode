

//var webserviceURL = "http://localhost:59993/BrokerService/BrokerService.asmx/BrokerMainForAndroid";
var deletemsgarrayCustomer = [];
var webserviceURL = "";
var messagelist = [];
var GlobleUserIDCustomer = "";
var editclick = "false";

function GetCustomerMessages(UserId, WeServiceUrl) {

    webserviceURL = WeServiceUrl;
   // alert("webserviceURL" + webserviceURL);

    GlobleUserIDCustomer = UserId;
    $.ajax({
        type: "POST",
        url: webserviceURL,
        data: { UserId: UserId, TimeStamp: "", ActionName: "DoGetMessages" },
        //contentType: "application/json; charset=utf-8",
        //dataType: "json",
        success: function (data) {
            //alert('Success data '+ data);
            var obj = JSON.parse(data);
           // alert(data);
            var issuccess = obj.IsSuccess;
            var ErrorMessage = obj.ErrorMessage;

            if (issuccess == true) {
                //alert('array '+messagelist);
                var ContactedMessageList = obj.ContactedMessageList;
                //alert('msg count :'+parseInt(ContactedMessageList.length));
                if (parseInt(ContactedMessageList.length) > 0) {

                    $.each(ContactedMessageList, function (i, res) {

                        if(messagelist.indexOf(res.MessageId) >=0)	
						{//alert('contains');
						}
						else{
                      //  alert(res);
                        var content = '';
                        var profileimg = '';

                        if (res.ProfilePictureImg != '') {

                            profileimg = res.ProfilePictureImg;
                        }
                        else {
                            profileimg = '../Images/Profile/customer.jpg';
                        }

                        //var dt = res.ContactDate;
                        var dt = res.LocalDateTime;
                        var dt1 = dt.split(' ');
                        var datedb = dt1[0];
                        var timedb = dt1[1];
                        var ampmdb = dt1[2];
                        msgdate = datedb;

                        var timeconvert = timedb.split(':');
                        var hrs = timeconvert[0];
                        var min = timeconvert[1];
                        var sec = timeconvert[2];



                        var nowdt = new Date();
                      
                        var nowdate = (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();
                        //var nowdate = nowdt.getDate() + '-' + month + '-' + nowdt.getFullYear();
                        //var nowtime = nowdt.getHours() + ":" + nowdt.getMinutes() + ":" + nowdt.getSeconds();
                        //var ampm = (nowdt.getHours() >= 12) ? "PM" : "AM";
                        var msgdate = '';


                        if (datedb == nowdate) {
                            //alert('datedb '+ datedb);
                            //alert('nowdate '+ nowdate);
                            msgdate = timedb;

                            //alert(hrs);
                            if (hrs > 12) {
                                hrs = hrs - 12;
                                //sec="AM";
                            }
                            else {
                                //sec="PM";

                            }
                            sec = ampmdb;

                            if (hrs <= 9) { hrs = "0" + hrs; }
                            if (min <= 9) { min = "0" + min; }


                            msgdate = hrs.slice(-2) + ':' + min.slice(-2) + ' ' + sec;

                            //msgdate=hrs+':'+min+' '+sec;
                            //alert("msdate"+msgdate);
                        }
                        else {
                            msgdate = datedb;
                        }

                        content = '<div id=' + res.MessageId + '  class="MessageList col-md-12 col-sm-12 col-xs-12 nopadding">'
                                + '<div style="display:none;" id="Hidden' + res.MessageId + '" class="BrokerName"> ' + res.BrokerName + ' </div>'
                            + '<div style="cursor: pointer;"  href="#" onclick="getChatMessageCustomer(\'' + res.BrokerMsgId + '\',\'' + res.MessageId + '\' ,\'' + res.BrokerId + '\',\'' + res.CustomerId + '\',\'' + res.BrokerName + '\', \'' + res.Message + '\',\'' + msgdate + '\')">'
                            + '<div class="col-md-2 col-sm-2 col-xs-2" style="text-align:left;padding-right:0;color:black;padding-top: 1%;">'
                                   + '<img class="circular--landscape circular--square circular--portrait" src=' + profileimg + ' id="profilepicprofile" style="position:none !important;" />'
                               + '</div>'
                           + '<div id="Name' + res.MessageId + '" class="col-md-3 col-sm-3 col-xs-3" style="text-align:left;padding-right:0;color:black;padding-top: 1%;">'
                                + res.BrokerName
                           + '</div>'
                           + '<div class="col-md-5 col-sm-5 col-xs-5 MainMessage" id="LatestMsg' + res.MessageId + '">'
                              + res.Message
                              
                             + '</div></div>'
                              + '<div id="Name' + res.MessageId + '" class="col-md-2 col-sm-2 col-xs-2" style="text-align:center;padding-right:0;color:black;padding-top: 1%;">'

                              + '<div class="col-md-12 col-sm-12 col-xs-12" ><span class="countmsg" id="Count' + res.MessageId + '" ></span></div>'
                                + '<div class="col-md-12 col-sm-12 col-xs-12"><span class="tickdiv"> <img src="../Images/Assets/redioButton.png" style="width:20px;height:20px;" id="tick' + res.MessageId + '" onclick="getCustomerDeletedItems(\'' + res.MessageId + '\')" class="tickimg"></span></div>'
                                + '<div class="col-md-12 col-sm-12 col-xs-12" ><span class="msgDate" id="date' + res.MessageId + '" >' + msgdate + '</span> </div>'
                             + '</div>'
                            + '</div>';


                 
                        $('#customerMeassage').prepend(content);

                            messagelist.push(res.MessageId);

                        
                        //CountMsg++;
                          
                        }
                    });

                    $('#Nocontent').text('No Messages Found !');

                }
                else {
                    if (messagelist.length <= 0) {
                        $('#Nocontent').text('No Messages Found !');
                    }
                }

                /*get latest message*/

                $.ajax({
                    type: "POST",
                    url: webserviceURL,
                    data: { UserId: UserId, ActionName: "DoGetUnreadMsgCount" },
                    success: function (data) {
                       // alert('data '+data);
                        var obj = JSON.parse(data);
                        var issuccess = obj.IsSuccess;
                        var ErrorMessage = obj.ErrorMessage;
                        var MessageDetails = obj.MessageDetails;
                        var ContactedMessageList = obj.ContactedMessageList;

                        if (issuccess == true) {
                            var totalunreadconver = 0;

                            $.each(MessageDetails, function (i, res) {
                                //.removeClass('test2');
                                if (parseInt(res.Cnt) >= 1) {
                                    totalunreadconver++;


                                    //alert(res.MessageId);
                                    jQuery('#Count' + res.MessageId).addClass('circlechatcountOnMessage');
                                    jQuery('#date' + res.MessageId).addClass('UnreadMsgdate');
                                    $('#Count' + res.MessageId).text(res.Cnt);


                                }

                                //var dt = res.ContactDate;
                                var dt = res.dtime;
                                var dt1 = dt.split(' ');
                                var datedb = dt1[0];
                                var timedb = dt1[1];
                                var ampmdb = dt1[2];
                                msgdate = datedb;


                                var timeconvert = timedb.split(':');
                                var hrs = timeconvert[0];
                                var min = timeconvert[1];
                                var sec = timeconvert[2];


                                var nowdt = new Date();
                                //var nowdate = (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();

                              
                                var nowdate = (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();
                              //  var nowdate = nowdt.getDate() + '-' + month + '-' + nowdt.getFullYear();

                                //var nowtime = nowdt.getHours() + ":" + nowdt.getMinutes() + ":" + nowdt.getSeconds();
                                //var ampm = (nowdt.getHours() >= 12) ? "PM" : "AM";
                                var msgdate = '';

                               

                                if (datedb == nowdate) {
                                    //alert('datedb '+ datedb);
                                    //alert('nowdate '+ nowdate);
                                    msgdate = timedb;

                                    //alert(hrs);
                                    if (hrs > 12) {
                                        hrs = hrs - 12;
                                        //sec="AM";
                                    }
                                    else {
                                        //sec="PM";

                                    }
                                    sec = ampmdb;

                                    if (hrs <= 9) { hrs = "0" + hrs; }
                                    if (min <= 9) { min = "0" + min; }


                                    msgdate = hrs.slice(-2) + ':' + min.slice(-2) + ' ' + sec;

                                    //msgdate=hrs+':'+min+' '+sec;
                                    //alert("msdate"+msgdate);
                                }
                                else {
                                    msgdate = datedb;
                                }
                                $('#date' + res.MessageId).text(msgdate);
                                $('#LatestMsg' + res.MessageId).text(res.BrokerMessage);

                            });

                            if (totalunreadconver > 0) {

                                jQuery('#totalunreadcount').addClass('circlechatcountheader');
                                $('#totalunreadcount').text(totalunreadconver);

                            }

                            //totalUnreadMsgGlobleBroker=totalunreadconver;
                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {

                    }
                });

            }

        },
        error: function (e) {
            //alert('GetCustomerMessages error: ' + e.msg);
        }
    });

   
}




function getChatMessageCustomer(BrokerMessageId, CustomerMessageId, BrokerId, CustomerId, CustomerName, Message, MsgDate) {

    deletemsgarrayCustomer = [];

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '/Chat/EncryptBrokerData',
        data: { BrokerMessageId: BrokerMessageId, CustomerMessageId: CustomerMessageId, BrokerId: BrokerId, CustomerId: CustomerId, CustomerName: CustomerName },
        success: function (Data) {
            //alert(Data.BrokerMessageId);
            window.location.href = "/Chat/CustomerChat?BrokerMessageId=" + Data.BrokerMessageId + "&CustomerMessageId=" + Data.CustomerMessageId + "&BrokerId=" + Data.BrokerId + "&CustomerId=" + Data.CustomerId + "&CustomerName=" + Data.CustomerName;

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        }
    });

    //window.location.href = "/Chat/CustomerChat?BrokerMessageId=" + BrokerMessageId + "&CustomerMessageId=" + CustomerMessageId + "&BrokerId=" + BrokerId + "&CustomerId=" + CustomerId + "&CustomerName=" + CustomerName;

}



$(function(){
    $("#deletemessages").click(function (e) {
      //  alert('called');
//$('#deletemessages').off("click").on("click", function () {
    if (editclick == "false") {
        $('.tickimg').css({ display: "block" });
        editclick = "true";
    }
    else {
        $('.tickimg').css({ display: "none" });
        editclick = "false";
    }
    });

    $("#deletemultiplemsg").click(function (e) {
        $("#myModal").modal('show');
    });


    // $('#deletemessagesconfirm').off("click").on("click", function () {

    $("#deletemessagesconfirm").click(function (e) {
        var listtodelete = deletemsgarrayCustomer.toString();

        $.ajax({
            type: "POST",
            url: webserviceURL,
            data: { UserId: GlobleUserIDCustomer, MessageId: listtodelete, ActionName: "DoDeleteMultipleMessage" },
            success: function (data) {
               // alert('data '+data);
                var obj = JSON.parse(data);
                var issuccess = obj.IsSuccess;
                var ErrorMessage = obj.ErrorMessage;
                if (issuccess == true) {
                    //DB Change
                                   
                    $('#deletemessages').hide();
                    $('#brokerSearchbtn').show();

                    deletemsgarrayCustomer = [];



                    $("#deletemultiplemsg").hide();//16Mar17
                    $("#deletemessages").show();//16Mar17
                    $('.tickimg').css({ display: "none" });//16Mar17
                    editclick = "false";

                    $('#myModal').modal('hide');

                    for (i = 0; i <= deletemsgarrayCustomer.length; i++) {
                      
                        $('#' + deletemsgarrayCustomer[i]).remove();

                        messagelist = jQuery.grep(messagelist, function (value) {
                            return value != deletemsgarrayBroker[i];
                        });
                    }

                    if (messagelist.length <= 0) {
                        $('#Nocontent').text('No Messages Found !');
                    }

                   
                   

                  //  $('#myModal').modal('hide');
                 
                }
                else if (issuccess == false) {
                    //$('.tickimg').css({ display: "none" });//16Mar17
                    $("#deletemultiplemsg").hide();//16Mar17
                    $("#deletemessages").show();//16Mar17
                    $('.tickimg').css({ display: "none" });//16Mar17
                    editclick = "false";

                    $('#myModal').modal('hide');

                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('#myModal').modal('hide');
            }
        });

    });

});




function getCustomerDeletedItems(id) {

    //alert(id);

    deletemsgarrayCustomer.push(id);
    if (parseInt(deletemsgarrayCustomer.length) > 0) {

        if (deletemsgarrayCustomer.indexOf(id) >= 0) {

            $('#tick' + id).attr('src', '../Images/Assets/radioBtn_checked.png');
            $("#deletemessages").hide();
            $("#deletemultiplemsg").show();

            //  alert('len1 ' + deletemsgarrayCustomer.length);

            for (var i = 0; i < deletemsgarrayCustomer.length; i++) {
                for (var j = 0; j < deletemsgarrayCustomer.length; j++) {
                    if (i != j) {
                        if (deletemsgarrayCustomer[i] == deletemsgarrayCustomer[j]) {

                            // alert('remove fun ' + deletemsgarrayCustomer[i]);
                            removeBrokerTick(deletemsgarrayCustomer[i]);

                        }


                    }
                }
            }

        }

    }

}


function removeBrokerTick(removeid) {

   // alert('remove');
    $('#tick'+removeid).attr('src','../Images/Assets/redioButton.png');
    //  alert(deletemsgarrayCustomer.length);
    deletemsgarrayCustomer = jQuery.grep(deletemsgarrayCustomer, function (value) {
                                 return value != removeid;
                                 });
    
    // alert(deletemsgarrayCustomer.length);
    if (parseInt(deletemsgarrayCustomer.length) <= 0)
    {
        //alert(deletemsgarrayCustomer.length);
    	$("#deletemessages").show();
        $("#deletemultiplemsg").hide();
    }
}

 /*get latest message*/
	






	setInterval(function()
				{ 	
	    	
        // Check Message is deleted from other device then remove msg .
	    $.ajax({
	        type: "POST",
	        url: webserviceURL,
	        data: { UserId: GlobleUserIDCustomer, TimeStamp: "", ActionName: "DoGetMessages" },
	        //contentType: "application/json; charset=utf-8",
	        //dataType: "json",
	        success: function (data) {
	            //alert('Success data '+ data);

	            var arrayList = [];
	            var obj = JSON.parse(data);
	            // alert(data);
	            var issuccess = obj.IsSuccess;
	            var ErrorMessage = obj.ErrorMessage;

	            if (issuccess == true) {
	                //alert('array '+messagelist);
	                var ContactedMessageList = obj.ContactedMessageList;
	                //alert('msg count :'+parseInt(ContactedMessageList.length));
	                if (parseInt(ContactedMessageList.length) > 0) {

	                    $.each(ContactedMessageList, function (i, res) {
	                        arrayList.push(res.MessageId);
	                    });
	                }
	                   
	                   // var TodeleteList = [];
	                    $.each(messagelist, function(idx, value){
	                        if ($.inArray(value, arrayList) == -1) {

	                            //TodeleteList.push(value);

	                            $('#' + value).remove();//remove

	                            messagelist = jQuery.grep(messagelist, function (value1) {
	                                return value1 != value;
	                            });
	                        }
	                    });

	                    if (messagelist.length <= 0) {
	                        $('#Nocontent').text('No Messages Found !');
	                    }

	                  
	                
	            }
	        },
	        error: function (XMLHttpRequest, textStatus, errorThrown) {

	           // alert('interval 1 error: ' + e.msg);
	        }
	    });


      //  alert('hi');
			$.ajax({		
				type: "POST",
				url: webserviceURL,
				data: { UserId: GlobleUserIDCustomer, ActionName: "DoGetUnreadMsgCount" },
				success: function (data) {	
					//alert('data '+data);
					var obj = JSON.parse(data);
					var issuccess = obj.IsSuccess;
					var ErrorMessage=obj.ErrorMessage;	
					var MessageDetails=obj.MessageDetails;
					
					var UnreadMsgCnt=obj.UnreadMsgCnt;
					
					var ContactedMessageList=obj.ContactedMessageList;
					var mainmsgarray=[];
					
					if(issuccess==true)
					{
						var totalunreadconver=0;
						
						if(parseInt(ContactedMessageList.length) > 0)
			       		{
							totalunreadconver=parseInt(ContactedMessageList.length);
							
							//alert(totalunreadconver);
							
                            //4June17
							$.each(messagelist, function (idx, value) {

							    jQuery('#Count' + value).removeClass('circlechatcountOnMessage');
							    jQuery('#date' + value).removeClass('UnreadMsgdate');
							    $('#Count' + value).text('');

							});

						$.each(ContactedMessageList, function (i, res) {
						
						
							mainmsgarray.push(res.MessageId);
							
							jQuery('#Count' + res.MessageId).addClass('circlechatcountOnMessage');
								jQuery('#date'+res.MessageId).addClass('UnreadMsgdate');
								$('#Count'+res.MessageId).text('1');
							
							if(messagelist.indexOf(res.MessageId) >=0)	
							{//alert('contains');
							}
							else{// alert('not contains '+ res.MessageId);
						var content='';
						var profileimg='';
						
							if(res.ProfilePictureImg!='')
								{
								
								profileimg=res.ProfilePictureImg;
								}
							else
								{
							    profileimg = '../Images/Profile/customer.jpg';
								}
							
							
							 	//var dt = res.ContactDate;
								var dt = res.LocalDateTime;
								var dt1=dt.split(' ');
								var datedb=dt1[0];
								var timedb=dt1[1];
								var ampmdb=dt1[2];
								msgdate=datedb;
								
												
								//alert();
								var timeconvert=timedb.split(':');
								var hrs=timeconvert[0];
								var min=timeconvert[1];
								var sec=timeconvert[2];
								
						
								
								var nowdt = new Date();
								
								var nowdate = (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();
								//var nowdate = nowdt.getDate() + '-' + month + '-' + nowdt.getFullYear();
								//var nowtime = nowdt.getHours() + ":" + nowdt.getMinutes() + ":" + nowdt.getSeconds();
								//var ampm = (nowdt.getHours() >= 12) ? "PM" : "AM";
								var msgdate='';
								
								if(datedb == nowdate)
								{
									//alert('datedb '+ datedb);
									//alert('nowdate '+ nowdate);
									msgdate=timedb;	
									
									//alert(hrs);
									if(hrs>12){
										hrs=hrs-12;
										//sec="AM";
									}
									else
									{
										//sec="PM";
										
									}
									sec=ampmdb;
									//alert('sec ' + sec);
									if(hrs<=9){hrs="0"+hrs;}
									if(min<=9){min="0"+min;}
									
									
									msgdate=hrs.slice(-2)+':'+min.slice(-2)+' '+sec;
									
									//msgdate=hrs+':'+min+' '+sec;
									//alert("msdate"+msgdate);
								}
								else
								{
								msgdate=datedb;
								}
								
								
								content = '<div id=' + res.MessageId + '  class="MessageList col-md-12 col-sm-12 col-xs-12 nopadding">'
                                + '<div style="display:none;" id="Hidden' + res.MessageId + '" class="BrokerName"> ' + res.BrokerName + ' </div>'
                            + '<div style="cursor: pointer;"  href="#" onclick="getChatMessageCustomer(\'' + res.BrokerMsgId + '\',\'' + res.MessageId + '\' ,\'' + res.BrokerId + '\',\'' + res.CustomerId + '\',\'' + res.BrokerName + '\', \'' + res.Message + '\',\'' + msgdate + '\')">'
                            + '<div class="col-md-2 col-sm-2 col-xs-2" style="text-align:left;padding-right:0;color:black;padding-top: 1%;">'
                                   + '<img class="circular--landscape circular--square circular--portrait" src=' + profileimg + ' id="profilepicprofile" style="position:none !important;" />'
                               + '</div>'
                           + '<div id="Name' + res.MessageId + '" class="col-md-3 col-sm-3 col-xs-3" style="text-align:left;padding-right:0;color:black;padding-top: 1%;">'
                                + res.BrokerName
                           + '</div>'
                           + '<div class="col-md-5 col-sm-5 col-xs-5 MainMessage" id="LatestMsg' + res.MessageId + '">'
                              + res.Message

                             + '</div></div>'
                              + '<div id="Name' + res.MessageId + '" class="col-md-2 col-sm-2 col-xs-2" style="text-align:center;padding-right:0;color:black;padding-top: 1%;">'

                              + '<div class="col-md-12 col-sm-12 col-xs-12" ><span class="countmsg" id="Count' + res.MessageId + '" ></span></div>'
                                + '<div class="col-md-12 col-sm-12 col-xs-12"><span class="tickdiv"> <img src="../Images/Assets/redioButton.png" style="width:20px;height:20px;" id="tick' + res.MessageId + '" onclick="getCustomerDeletedItems(\'' + res.MessageId + '\')" class="tickimg"></span></div>'
                                + '<div class="col-md-12 col-sm-12 col-xs-12" ><span class="msgDate" id="date' + res.MessageId + '" >' + msgdate + '</span> </div>'
                             + '</div>'
                            + '</div>';




							  
								$('#customerMeassage').prepend(content);

								messagelist.push(res.MessageId);
								
											
							
							}
							//$('#IsRead'+res.MessageId).text(res.IsRead);
							
							
						 });
			       		}
						
						var UnreadMsgArray = [];
						$.each(MessageDetails, function (i, res) {
							//.removeClass('test2');
							
						  
							var msgcountevery=0;
							if(mainmsgarray.indexOf(res.MessageId) <0)
							{
								if(parseInt(res.Cnt) >0)
								{
								    totalunreadconver++;
								    UnreadMsgArray.push(res.MessageId);
								}

							//alert('if'+ mainmsgarray);
							
							}
							
							$.each(ContactedMessageList, function (i, res1) {
							    UnreadMsgArray.push(res1.MessageId);
								if(parseInt(res.MessageId) == parseInt(res1.MessageId))
								{
									msgcountevery++
								}
							
							});
							
							
							msgcountevery=parseInt(res.Cnt) + parseInt(msgcountevery);
							
							if(parseInt(msgcountevery) >=1)
							{
								//totalunreadconver++;
								
								//var dt = res.ContactDate;
								
								
								//alert(res.MessageId);
							    jQuery('#Count' + res.MessageId).addClass('circlechatcountOnMessage');
								jQuery('#date'+res.MessageId).addClass('UnreadMsgdate');
								$('#Count'+res.MessageId).text(msgcountevery);
								
								
							
							}
							
							var dt = res.dtime;
							var dt1=dt.split(' ');
							var datedb=dt1[0];
							var timedb=dt1[1];
							var ampmdb=dt1[2];
							msgdate=datedb;
							var timeconvert=timedb.split(':');
							var hrs=timeconvert[0];
							var min=timeconvert[1];
							var sec=timeconvert[2];
							
															
							var nowdt = new Date();
						
							var nowdate = (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();
							//var nowdate = nowdt.getDate() + '-' + month + '-' + nowdt.getFullYear();

							//var nowdate= (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();
							//var nowtime = nowdt.getHours() + ":" + nowdt.getMinutes() + ":" + nowdt.getSeconds();
							//var ampm = (nowdt.getHours() >= 12) ? "PM" : "AM";
							var msgdate='';
							
							

							if(datedb == nowdate)
							{
								//alert('datedb '+ datedb);
								//alert('nowdate '+ nowdate);
								msgdate=timedb;	
							
								//alert(hrs);
								if(hrs>12){
									hrs=hrs-12;
									//sec="AM";
								}
								else
								{
									//sec="PM";
									
								}
								sec=ampmdb;
								
								if(hrs<=9){hrs="0"+hrs;}
								if(min<=9){min="0"+min;}

								msgdate=hrs.slice(-2)+':'+min.slice(-2)+' '+sec;
								
								//msgdate=hrs+':'+min+' '+sec;
								//alert("msdate"+msgdate);
							}
							else
							{
							msgdate=datedb;
							}
							
							$('#date'+res.MessageId).text(msgdate);
							$('#LatestMsg' + res.MessageId).text(res.BrokerMessage);
							
							
							
						});
						if (MessageDetails.length > 0) {
						    $.each(messagelist, function (idx, value) {
						        if ($.inArray(value, UnreadMsgArray) == -1) {

						            //alert('value '+value);
						            //TodeleteList.push(value);

						            jQuery('#Count' + value).removeClass('circlechatcountOnMessage');
						            jQuery('#date' + value).removeClass('UnreadMsgdate');
						            $('#Count' + value).text('');

						        }
						    });
						}

						if (UnreadMsgCnt.length > 0) {
						$.each(UnreadMsgCnt, function (i, res) {
							
							
							
							if(res.UnreadMsgCnt >0)
							{
							jQuery('#totalunreadcount').addClass('circlechatcountheader');
							$('#totalunreadcount').text(res.UnreadMsgCnt);
							
							
							}
							else
								{
								jQuery('#totalunreadcount').removeClass('circlechatcountheader');
								$('#totalunreadcount').text('');
								
								
								}
							
						});
						}
						else {
						    jQuery('#totalunreadcount').removeClass('circlechatcountheader');
						    $('#totalunreadcount').text('');
						}
						
						
						//totalUnreadMsgGlobleBroker=totalunreadconver;
					}
					
					},  					
				error : function(XMLHttpRequest, textStatus, errorThrown) {
				    //alert('interval 2 error: ' + e.msg);
				}			 			
			});
				}, 2000);
	//alert('interval');
	/*get latest message*/